namespace Texter.ViewModels
{
    public class GroupItem : ViewModels.ViewModelBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                OnPropertyChanged();
            }
        }

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
