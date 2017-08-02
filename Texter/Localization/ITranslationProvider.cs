namespace Texter.Localization
{
    public interface ITranslationProvider
    {
        /// <summary>
        /// Translates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object Translate(string key);

        /// <summary>
        /// Gets the available languages.
        /// </summary>
        /// <value>The available languages.</value>
        System.Collections.Generic.IEnumerable<System.Globalization.CultureInfo> Languages { get; }

    }
}
