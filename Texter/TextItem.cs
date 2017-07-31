﻿namespace Texter
{
    public class TextItem : ViewModels.ViewModelBase
    {
        private int _groupID;
        public int GroupID
        {
            get { return _groupID; }
            set
            {
                if (_groupID == value) return;
                _groupID = value;
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
