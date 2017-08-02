using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Texter.Localization
{
    public class TranslationManager
    {
        private static TranslationManager _translationManager;

        public event EventHandler LanguageChanged;

        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value != Thread.CurrentThread.CurrentUICulture)
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnLanguageChanged();
                }
            }
        }

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                return TranslationProvider?.Languages ?? Enumerable.Empty<CultureInfo>();
            }
        }

        public static TranslationManager Instance
        {
            get
            {
                return _translationManager ?? (_translationManager = new TranslationManager());
            }
        }

        public ITranslationProvider TranslationProvider { get; set; }

        private void OnLanguageChanged()
        {
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public object Translate(string key)
        {
            if (TranslationProvider != null)
            {
                object translatedValue = TranslationProvider.Translate(key);
                if (translatedValue != null)
                {
                    return translatedValue;
                }
            }
            return string.Format("!{0}!", key);
        }

        public string TranslateString(string key)
        {
            return Translate(key) as string;
        }
    }
}
