using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Texter.Logger;

namespace Texter
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<TextItem> _textItems;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                EventManager.RegisterClassHandler(typeof(ListBoxItem), MouseLeftButtonDownEvent, new RoutedEventHandler(TextItemClicked));
            }
            catch (Exception ex)
            {
                string error = "Sikertelen az alkalmazás inicializálása. Kérem, indítsa újra. [ERR400]";
                LogHelper.LogException(ex, error);
                MessageBox.Show(error);
            }
        }

        private void AddNewItemEvent(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputText.Text)) return;
            _textItems.Add(new TextItem { Text = InputText.Text });
            InputText.Text = null;
        }

        private void OnRemoveEvent(object sender, RoutedEventArgs e)
        {
            TextItem textItem = (sender as FrameworkElement)?.DataContext as TextItem;
            if (textItem == null) return;
            _textItems.Remove(textItem);
        }

        private async void TextItemClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                TextItem textItem = (sender as ListBoxItem)?.DataContext as TextItem;
                if (textItem == null) return;
                this.Expander.IsExpanded = false;

                WindowState windowState = WindowState;

                Hide();
                WindowState = WindowState.Minimized;
                await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(200));

                IntPtr targetWindow = await Win32Wrapper.PasteText(textItem.Text);

                WindowState = windowState;
                Show();

                Win32Wrapper.SetForegroundWindow(targetWindow);
            }
            catch (Exception ex)
            {
                string error = "Sikertelen a szöveg beillesztése. [ERR202]";
                LogHelper.LogException(ex, error);
                MessageBox.Show(error);
            }

        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeToContent = System.Windows.SizeToContent.Height;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            try
            {
                TextList.ItemsSource = _textItems = System.Threading.Tasks.Task.Run(async () => await FileManager.LoadConfigAsync<ObservableCollection<TextItem>>()).Result ?? new ObservableCollection<TextItem>();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            try
            {
                FileManager.SaveConfigAsync(_textItems).Wait();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }
    }
}
