using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Texter.Localization
{
    /// <summary>
    /// The tanslate markup extension knows the resource key and provides the translated value. 
    /// It listens to the LanguageChanged event of the translation manager and updates its value. 
    /// This event handler is implemented by the weak event pattern to prevent memory leaks.
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        private string _key;

        public TranslateExtension(string key)
        {
            _key = key;
        }

        [ConstructorArgument("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding("Value")
            {
                Source = new TranslationData(_key)
            };
            return binding.ProvideValue(serviceProvider);
        }
    }

}
