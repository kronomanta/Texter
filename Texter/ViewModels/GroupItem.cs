namespace Texter.ViewModels
{
    public class GroupItem : ViewModels.ViewModelBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                OnPropertyChanged();
            }
        }
    }
}
