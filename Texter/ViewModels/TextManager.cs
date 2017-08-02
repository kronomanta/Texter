using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using Texter.Intefaces;
using Texter.Logger;

namespace Texter.ViewModels
{
    public partial class TextManagerViewModel : ViewModelBase
    {
        private readonly IConfirmer _confirmer;
        private readonly IWindowBase _window;

        private ObservableCollection<KeyValuePair<GroupItem, ObservableCollection<TextItem>>> _items;
        public ObservableCollection<KeyValuePair<GroupItem, ObservableCollection<TextItem>>> Items
        {
            get { return _items; }
            set
            {
                if (_items == value) return;
                _items = value;
                OnPropertyChanged();
            }
        }

        private GroupItem _selectedGroup;
        public GroupItem SelectedGroup
        {
            get { return _selectedGroup; }
            set
            {
                if (_selectedGroup == value) return;
                _selectedGroup = value;
                OnPropertyChanged();
                AddTextCommand.RaiseCanExecuteChanged();
            }
        }

        private string _textInput;
        public string TextInput
        {
            get { return _textInput; }
            set
            {
                if (_textInput == value) return;
                _textInput = value;
                OnPropertyChanged();
                AddTextCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _keepOnClipboardAfterInsert;
        public bool KeepOnClipboardAfterInsert
        {
            get { return _keepOnClipboardAfterInsert; }
            set
            {
                if (_keepOnClipboardAfterInsert == value) return;
                _keepOnClipboardAfterInsert = value;
                OnPropertyChanged();
            }
        }

        public TextManagerViewModel(IConfirmer confirmer, IWindowBase window)
        {
            _confirmer = confirmer;
            _window = window;
        }

        private void AddText()
        {
            if (string.IsNullOrWhiteSpace(TextInput)) return;

            GroupItem groupItem = SelectedGroup ?? Items.Where(x => string.IsNullOrEmpty(x.Key.Text)).Select(x => x.Key).FirstOrDefault() ?? new GroupItem();

            var group = Items.Where(x => x.Key == groupItem).Select(x => x.Value).FirstOrDefault();
            if (group == null)
            {
                Items.Add(new KeyValuePair<GroupItem, ObservableCollection<TextItem>>(groupItem, group = new ObservableCollection<TextItem>()));
            }

            group.Add(new TextItem { Text = TextInput });
            TextInput = null;
            SelectedGroup = null;
        }

        private void RemoveText(TextItem item)
        {
            if (item == null) return;
            foreach (var group in Items)
            {
                if (group.Value.Remove(item)) return;
            }
        }

        private void AddGroup()
        {
            Items.Add(new KeyValuePair<GroupItem, ObservableCollection<TextItem>>(new GroupItem(), new ObservableCollection<TextItem>()));
        }

        private void RemoveGroupItem(GroupItem item)
        {
            if (item == null) return;

            bool confirmed = true;
            if (Items.Single(x => x.Key == item).Value.Count > 0)
            {
                confirmed = _confirmer.ConfirmYesNo(
                    string.Format(Localization.TranslationManager.Instance.TranslateString("DeleteGroupWithConnectionElements"), item.Text),
                    Localization.TranslationManager.Instance.TranslateString("DeleteGroupWithConnectionElementsHeader"));
            }

            if (confirmed)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Key == item)
                    {
                        Items.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private async void PasteFromClipboard(TextItem textItem)
        {
            try
            {
                if (textItem == null) return;

                _window.HideWithPrevStatePreserved();

                await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(200));

                IntPtr targetWindow = await Win32Wrapper.PasteText(textItem.Text, KeepOnClipboardAfterInsert);

                _window.ShowWithPreservedState();

                Win32Wrapper.SetForegroundWindow(targetWindow);
            }
            catch (Exception ex)
            {
                string error = Localization.TranslationManager.Instance.TranslateString("ErrorMessagePasterFromClipboard");
                LogHelper.LogException(ex, error);
                _confirmer.ConfirmStop(error);
            }
        }

        public void SaveItems()
        {
            try
            {
                SaveConfigToFileAsync(null);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        public void LoadItems()
        {
            try
            {
                LoadConfigFromFileAsync(null);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }


        private async System.Threading.Tasks.Task OpenConfig()
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    CheckFileExists = true,
                    Filter = "Texter file (*.ttr)|*.ttr"
                };

                if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.FileName))
                {
                    await LoadConfigFromFileAsync(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                _confirmer.ConfirmStop(ex.Message);
            }
        }

        private async System.Threading.Tasks.Task SaveConfig()
        {
            try
            {
                SaveFileDialog dlg = new SaveFileDialog
                {
                    ValidateNames = true,
                    AddExtension = true,
                    DefaultExt = "ttr",
                    Filter = "Texter file (*.ttr)|*.ttr",
                    FileName = "Texter_" + DateTime.Now.ToString("yyyyMMdd_HHmm")
                };

                if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.FileName))
                {
                    await SaveConfigToFileAsync(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                _confirmer.ConfirmStop(ex.Message);
            }
        }

        private async System.Threading.Tasks.Task SaveConfigToFileAsync(string path)
        {
            await FileManager.SaveConfigAsync(new Config { Groups = Items.Select(x => new ItemHolder { GroupItem = x.Key, TextItems = x.Value.ToArray() }).ToArray() }, path);
        }

        private async System.Threading.Tasks.Task LoadConfigFromFileAsync(string path)
        {
            Config config;

            try
            {
                config = (await FileManager.LoadConfigAsync<Config>(path)) ?? new Config();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
                if (!string.IsNullOrWhiteSpace(path))
                {
                    //import error
                    _confirmer.ConfirmStop(Localization.TranslationManager.Instance.TranslateString("ErrorMessageInvalidFileFormat"));
                    return;
                }
                config = new Config();
            }

            if (config.Groups == null) config.Groups = new ItemHolder[0];
            Items = new ObservableCollection<KeyValuePair<GroupItem, ObservableCollection<TextItem>>>();
            foreach (var group in config.Groups)
            {
                Items.Add(new KeyValuePair<GroupItem, ObservableCollection<TextItem>>(group.GroupItem, new ObservableCollection<TextItem>(group.TextItems ?? new TextItem[0])));
            }
        }

    }
}
