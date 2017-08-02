using System;
using System.ComponentModel;
using System.Windows;

namespace Texter.Localization
{
    public class TranslationData : ViewModels.ViewModelBase, IWeakEventListener, IDisposable
    {
        private string _key;

        public TranslationData(string key)
        {
            _key = key;
            LanguageChangedEventManager.AddListener(
                      TranslationManager.Instance, this);
        }

        ~TranslationData()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                LanguageChangedEventManager.RemoveListener(
                          TranslationManager.Instance, this);
            }
        }


        public object Value
        {
            get
            {
                return TranslationManager.Instance.Translate(_key);
            }
        }

        public bool ReceiveWeakEvent(Type managerType,
                                object sender, EventArgs e)
        {
            if (managerType == typeof(LanguageChangedEventManager))
            {
                OnPropertyChanged("Value");
                return true;
            }
            return false;
        }
    }

}
