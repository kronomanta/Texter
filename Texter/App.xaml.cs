using System;
using System.Windows;
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
        }

        private void HandleUnhandledException(Exception e)
        {
            string error = "Végzetes hiba történt. [ERR5002]";
            LogHelper.LogException(e, error);
            MessageBox.Show(error);
            Environment.Exit(-1);
        }
    }
}
