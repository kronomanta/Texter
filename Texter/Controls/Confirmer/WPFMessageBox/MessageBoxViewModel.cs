using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Texter.ViewModels;

namespace Texter.Controls.Confirmer.WPFMessageBox
{
    public class MessageBoxViewModel : ViewModelBase
    {
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public string InnerMessageDetails
        {
            get { return _innerMessageDetails; }
            set
            {
                if (_innerMessageDetails == value) return;
                _innerMessageDetails = value;
                OnPropertyChanged();
            }
        }

        public string YesText
        {
            get { return _yesText; }
            set
            {
                if (_yesText == value) return;
                _yesText = value;
                OnPropertyChanged();
            }
        }

        public string NoText
        {
            get { return _noText; }
            set
            {
                if (_noText == value) return;
                _noText = value;
                OnPropertyChanged();
            }
        }

        public string CancelText
        {
            get { return _cancelText; }
            set
            {
                if (_cancelText == value) return;
                _cancelText = value;
                OnPropertyChanged();
            }
        }

        public string OKText
        {
            get { return _okText; }
            set
            {
                if (_okText == value) return;
                _okText = value;
                OnPropertyChanged();
            }
        }

        public string CloseText
        {
            get { return _closeText; }
            set
            {
                if (_closeText == value) return;
                _closeText = value;
                OnPropertyChanged();
            }
        }

        public ImageSource MessageImageSource
        {
            get { return _messageImageSource; }
            set
            {
                _messageImageSource = value;
                OnPropertyChanged();
            }
        }

        public Visibility YesNoVisibility
        {
            get { return _yesNoVisibility; }
            set
            {
                if (_yesNoVisibility == value) return;
                _yesNoVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set
            {
                if (_cancelVisibility == value) return;
                _cancelVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility OkVisibility
        {
            get { return _okVisibility; }
            set
            {
                if (_okVisibility == value) return;
                _okVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility CloseVisibility
        {
            get { return _closeVisibility; }
            set
            {
                if (_closeVisibility == value) return;
                _closeVisibility = value;
                OnPropertyChanged();
            }
        }

        public Visibility ShowDetails
        {
            get { return _showDetails; }
            set
            {
                if (_showDetails == value) return;
                _showDetails = value;
                OnPropertyChanged();
            }
        }

        public ICommand YesCommand
        {
            get
            {
                return _yesCommand ?? (_yesCommand = new RelayCommand(() => SetDialogResultAndClose(WPFMessageBoxResult.Yes)));
            }
        }

        public ICommand NoCommand
        {
            get
            {
                return _noCommand ?? (_noCommand = new RelayCommand(() => SetDialogResultAndClose(WPFMessageBoxResult.No)));
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(() => SetDialogResultAndClose(WPFMessageBoxResult.Cancel)));
            }
        }
        
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand(() => SetDialogResultAndClose(WPFMessageBoxResult.Close)));
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new RelayCommand(() => SetDialogResultAndClose(WPFMessageBoxResult.Ok)));
            }
        }

        private void SetDialogResultAndClose(WPFMessageBoxResult result)
        {
            _view.Result = result;
            _view.Close();
        }

        public MessageBoxViewModel(WPFMessageBox view,
            string title, string message, string innerMessage,
            WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image, Dictionary<WPFMessageBoxResult, string> buttons)
            : this(view, title, message, innerMessage, buttonOption, image)
        {
            if (buttons == null || buttons.Keys.Count == 0) return;

            if (buttons.ContainsKey(WPFMessageBoxResult.Cancel))
            {
                CancelText = buttons[WPFMessageBoxResult.Cancel];
            }

            if (buttons.ContainsKey(WPFMessageBoxResult.Close))
            {
                CloseText = buttons[WPFMessageBoxResult.Close];
            }

            if (buttons.ContainsKey(WPFMessageBoxResult.No))
            {
                NoText = buttons[WPFMessageBoxResult.No];
            }

            if (buttons.ContainsKey(WPFMessageBoxResult.Ok))
            {
                OKText = buttons[WPFMessageBoxResult.Ok];
            }

            if (buttons.ContainsKey(WPFMessageBoxResult.Yes))
            {
                YesText = buttons[WPFMessageBoxResult.Yes];
            }
        }

        public MessageBoxViewModel(WPFMessageBox view, 
            string title, string message, string innerMessage,
            WPFMessageBoxButtons buttonOption, WPFMessageBoxImage image)
        {
            Title = title;
            Message = message;
            InnerMessageDetails = innerMessage;
            InitDefaultButtonTexts();

            SetButtonVisibility(buttonOption);
            SetImageSource(image);
            _view = view;
        }

        private void InitDefaultButtonTexts()
        {
            var translator = Localization.TranslationManager.Instance;

            _yesText = translator.TranslateString("YesText");
            _noText = translator.TranslateString("NoText");
            _cancelText = translator.TranslateString("CancelText");
            _okText = translator.TranslateString("OKText");
            _closeText = translator.TranslateString("CloseText");
        }

        private void SetButtonVisibility(WPFMessageBoxButtons buttonOption)
        {
            switch (buttonOption)
            {
                case WPFMessageBoxButtons.YesNo:
                    OkVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.YesNoCancel:
                    OkVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.OK:
                    YesNoVisibility = CancelVisibility = CloseVisibility = Visibility.Collapsed;
                    break;
                case WPFMessageBoxButtons.OKClose:
                    YesNoVisibility = CancelVisibility = Visibility.Collapsed;
                    break;
                default:
                    OkVisibility = CancelVisibility = YesNoVisibility = Visibility.Collapsed;
                    break;
            }

            ShowDetails = string.IsNullOrEmpty(InnerMessageDetails) ? Visibility.Collapsed : Visibility.Visible;
        }

        void SetImageSource(WPFMessageBoxImage image)
        {
            string folder = "pack://application:,,,/Texter;component/Controls/Confirmer/WPFMessageBox/Images/";
            string __Source = folder + "Default.png";
            switch (image)
            {
                case WPFMessageBoxImage.Alert:
                    __Source = folder + "Alert.png";
                    break;
                case WPFMessageBoxImage.Error:
                    __Source = folder + "Error.png";
                    break;
                case WPFMessageBoxImage.Information:
                    __Source = folder + "Info.png";
                    break;
                case WPFMessageBoxImage.OK:
                    __Source = folder + "OK.png";
                    break;
                case WPFMessageBoxImage.Question:
                    __Source = folder + "Help.png";
                    break;
                default:
                    __Source = folder + "Default.png";
                    break;

            }
            Uri __ImageUri = new Uri(__Source, UriKind.RelativeOrAbsolute);
            MessageImageSource = new BitmapImage(__ImageUri);
        }

        string _title;
        string _message;
        string _innerMessageDetails;

        private string _yesText = "Igen";
        private string _noText = "Nem";
        private string _cancelText = "Mégsem";
        private string _okText = "OK";
        private string _closeText = "Bezárás";

        Visibility _yesNoVisibility;
        Visibility _cancelVisibility;
        Visibility _okVisibility;
        Visibility _closeVisibility;
        Visibility _showDetails;

        ICommand _yesCommand;
        ICommand _noCommand;
        ICommand _cancelCommand;
        ICommand _closeCommand;
        ICommand _okCommand;

        WPFMessageBox _view;
        ImageSource _messageImageSource;
    }
}
