using System.Collections.Generic;

namespace Texter.Localization
{
    public class ResxTranslationProvider : ITranslationProvider
    {
        #region Private Members

        private readonly System.Resources.ResourceManager _resourceManager;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly.</param>
        public ResxTranslationProvider(string baseName, System.Reflection.Assembly assembly)
        {
            _resourceManager = new System.Resources.ResourceManager(baseName, assembly);
        }

        #endregion

        #region ITranslationProvider Members

        /// <summary>
        /// See <see cref="ITranslationProvider.Translate" />
        /// </summary>
        public object Translate(string key) => _resourceManager.GetString(key);

        #endregion

        #region ITranslationProvider Members

        /// <summary>
        /// See <see cref="ITranslationProvider.AvailableLanguages" />
        /// </summary>
        public IEnumerable<System.Globalization.CultureInfo> Languages
        {
            get
            {
                // TODO: Resolve the available languages
                yield return new System.Globalization.CultureInfo("hu");
                yield return new System.Globalization.CultureInfo("en");
            }
        }

        #endregion
    }
}
