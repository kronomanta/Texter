using System.Collections.Generic;

namespace Texter.Controls.Confirmer.WPFMessageBox
{
    public partial class WPFMessageBox
    {
        private static int windowCounter = 0;

        public WPFMessageBox()
        {
            InitializeComponent();
            Topmost = true;
        }

        public WPFMessageBoxResult Result { get; set; }

        public WPFMessageBoxResult Show(string message)
        {
            return Show(string.Empty, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public WPFMessageBoxResult Show(string title, string message)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public WPFMessageBoxResult Show(string title, string message, string details)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Default);
        }

        public WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            return Show(title, message, string.Empty, buttonOption, WPFMessageBoxImage.Default, buttonTexts);
        }

        public WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            return Show(title, message, details, buttonOption, WPFMessageBoxImage.Default, buttonTexts);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxImage image, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            return Show(title, message, string.Empty, WPFMessageBoxButtons.OK, image, buttonTexts);
        }

        public WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxImage image, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            return Show(title, message, details, WPFMessageBoxButtons.OK, image, buttonTexts);
        }

        public static WPFMessageBoxResult Show(string title, string message, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            return Show(title, message, string.Empty, buttonOption, image, buttonTexts);
        }

        private static readonly object lockObj = new object();
        public static WPFMessageBoxResult Show(string title, string message, string details, WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image, Dictionary<WPFMessageBoxResult, string> buttonTexts = null)
        {
            int windowId = 0;
            lock(lockObj)
                windowId = windowCounter++;

            var view = new WPFMessageBox();
            var viewModel = new MessageBoxViewModel(view, title, message, details, buttonOption, image, buttonTexts);
            view.DataContext = viewModel;
            
            view.ShowDialog();

            return view.Result;
        }

        protected override void OnSourceInitialized(System.EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }
    }
}
