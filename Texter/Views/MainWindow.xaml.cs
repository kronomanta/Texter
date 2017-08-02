using System;
using System.Windows;
using System.Windows.Input;
using Texter.Intefaces;
using Texter.Logger;
using Texter.ViewModels;

namespace Texter.Views
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : IWindowBase
    {
        private IConfirmer _confirmer;
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
                string error = Localization.TranslationManager.Instance.TranslateString("ErrorMessageInitialization");
                LogHelper.LogException(ex, error);
                confirmer.ConfirmStop(error);
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
                _confirmer = new Controls.Confirmer.MessageBoxConfirmer();

                DataContext = new TextManagerViewModel(_confirmer, this);

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

        private void ResizeGripPreviewMouseDown(object sender, MouseButtonEventArgs e)
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
