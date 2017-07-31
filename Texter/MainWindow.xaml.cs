using System;
using System.Windows;
using System.Windows.Input;
using Texter.Intefaces;
using Texter.Logger;
using Texter.View;
using Texter.ViewModels;

namespace Texter
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : IWindowBase
    {
        private IntPtr _windowHandle;

        private WindowState _tempPrevWindowState;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Subscribe();
            }
            catch (Exception ex)
            {
                string error = "Sikertelen az alkalmazás inicializálása. Kérem, indítsa újra. [ERR400]";
                LogHelper.LogException(ex, error);
                MessageBox.Show(error);
                Close();
            }
        }

        private void Subscribe()
        {
            Header.MouseDown += HeaderMouserLeftDown;

            CloseButton.Click += CloseButtonClicked;
            ResizedGrip.PreviewMouseDown += ResizeGripPreviewMouseDown;
            ResizedGrip.PreviewMouseUp += ResizeGripPreviewMouseUp;
        }
        
        private void Unsubcribe()
        {
            Header.MouseDown -= HeaderMouserLeftDown;

            CloseButton.Click -= CloseButtonClicked;
            ResizedGrip.PreviewMouseDown -= ResizeGripPreviewMouseDown;
            ResizedGrip.PreviewMouseUp -= ResizeGripPreviewMouseUp;

        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            try
            {
                DataContext = new TextManagerViewModel(new Confirmer(), this);

                ((TextManagerViewModel)DataContext).LoadItems();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                ((TextManagerViewModel)DataContext).SaveItems();

                Unsubcribe();
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex);
            }

            base.OnClosed(e);
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            _windowHandle = ((System.Windows.Interop.HwndSource)PresentationSource.FromVisual(this)).Handle;
        }

        private void ResizeGripPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Win32Wrapper.ResizeWindow(_windowHandle, Win32Wrapper.ResizeDirection.Right);
        }


        private void ResizeGripPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SizeToContent = SizeToContent.Height;
        }


        private void HeaderMouserLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        
        public void ShowWithPreservedState()
        {
            WindowState = _tempPrevWindowState;
            Show();

        }

        public void HideWithPrevStatePreserved()
        {
            _tempPrevWindowState = WindowState;

            Hide();
            WindowState = WindowState.Minimized;
        }

    }
}
