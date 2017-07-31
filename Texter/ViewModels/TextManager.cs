﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Texter.Intefaces;
using Texter.Logger;

namespace Texter.ViewModels
{
    public partial class TextManagerViewModel : ViewModelBase
    {
        private readonly IConfirmer _confirmer;
        private readonly IWindowBase _window;
        private int _groupIdCounter = 0;

        private ObservableCollection<TextItem> _textItems;
        public ObservableCollection<TextItem> TextItems
        {
            get { return _textItems; }
            private set
            {
                if (_textItems == value) return;
                _textItems = value;
                OnPropertyChanged();
                AddTextCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<GroupItem> _groupItems;
        public ObservableCollection<GroupItem> GroupItems
        {
            get { return _groupItems; }
            private set
            {
                if (_groupItems == value) return;
                _groupItems = value;
                OnPropertyChanged();
            }
        }

        private int? _selectedGroupID;
        public int? SelectedGroupID
        {
            get { return _selectedGroupID; }
            set
            {
                if (_selectedGroupID == value) return;
                _selectedGroupID = value;
                OnPropertyChanged();
                AddTextCommand.RaiseCanExecuteChanged();
            }
        }

        private string _textInput;
        public string TextInput
        { get { return _textInput; }
        set
            {
                 if (_textInput == value) return;
                _textInput = value;
                OnPropertyChanged();
                AddTextCommand.RaiseCanExecuteChanged();
            }
        }

        private string _groupInput;
        public string GroupInput
        {
            get { return _groupInput; }
            set
            {
                if (_groupInput == value) return;
                _groupInput = value;
                OnPropertyChanged();
                AddGroupCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _keepOnClipboardAfterInsert;
        public bool KeepOnClipboardAfterInsert
        {
            get { return _keepOnClipboardAfterInsert; }
            set { if (_keepOnClipboardAfterInsert == value) return;
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
            if (string.IsNullOrWhiteSpace(TextInput) || SelectedGroupID == null) return;
            TextItems.Add(new TextItem { Text = TextInput, GroupID = SelectedGroupID.Value });
            TextInput = null;
            SelectedGroupID = null;
        }

        private void RemoveText(TextItem item)
        {
            if (item == null) return;
            TextItems.Remove(item);
        }

        private void AddGroup()
        {
            if (string.IsNullOrWhiteSpace(GroupInput)) return;
            GroupItems.Add(new GroupItem { Text = GroupInput, ID = _groupIdCounter++ });
            GroupInput = null;
        }

        private void RemoveGroupItem(GroupItem item)
        {
            if (item == null) return;

            bool confirmed = true;
            if (TextItems.Any(x => x.GroupID == item.ID))
                confirmed = _confirmer.ConfirmYesNo($"Tartozik elem ehhez a csoporthoz: {item.Text}. Mégis törli?", "Kapcsolódó elemek törlése");

            if (confirmed)
            {
                GroupItems.Remove(item);
                TextItems = new ObservableCollection<TextItem>(TextItems.Where(x => x.GroupID != item.ID));
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
                string error = "Sikertelen a szöveg beillesztése. [ERR202]";
                LogHelper.LogException(ex, error);
                _confirmer.ConfirmStop(error, "Hiba");
            }
        }

        public void SaveItems()
        {
            try
            {
                FileManager.SaveConfigAsync(new Config { GroupItems = GroupItems.ToArray(), TextItems = TextItems.ToArray() }).Wait();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        public void LoadItems()
        {
            Config config;

            try
            {
                config = System.Threading.Tasks.Task.Run(async () => await FileManager.LoadConfigAsync<Config>()).Result ?? new Config();
            }
            catch(Exception ex)
            {
                config = new Config();
                LogHelper.LogException(ex);
            }

            GroupItems = new ObservableCollection<GroupItem>(config.GroupItems ?? new GroupItem[0]);
            TextItems = new ObservableCollection<TextItem>(config.TextItems ?? new TextItem[0]);

            if (GroupItems.Count > 0)
                _groupIdCounter = GroupItems[GroupItems.Count - 1].ID;
        }
    }
}
