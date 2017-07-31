namespace Texter.ViewModels
{
    public class TextItem : ViewModelBase
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

        private string _groupName;
        public string GroupName
        {
            get { return _groupName; }
            set
            {
                if (_groupName == value) return;
                _groupName = value;
                OnPropertyChanged();
            }
        }

        public TextItem(string text, GroupItem group)
        {
            Text = text;
            GroupName = group?.Text;
        }
    }
}
