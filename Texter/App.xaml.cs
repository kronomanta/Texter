using System;
using System.Windows;
using Texter.Localization;
using Texter.Logger;

namespace Texter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            DispatcherUnhandledException += (s,e) => HandleUnhandledException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (s,e) => HandleUnhandledException(e.ExceptionObject as Exception);

            TranslationManager.Instance.TranslationProvider = new ResxTranslationProvider("Texter.Resources.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        private void HandleUnhandledException(Exception e)
        {
            string error = TranslationManager.Instance.TranslateString("ErrorMessageFatal");
            LogHelper.LogException(e, error);
            new Controls.Confirmer.MessageBoxConfirmer().ConfirmStop(error);
            Environment.Exit(-1);
        }
    }
}
