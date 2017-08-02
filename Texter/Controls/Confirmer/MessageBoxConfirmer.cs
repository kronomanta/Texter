using System.Collections.Generic;
using Texter.Controls.Confirmer.WPFMessageBox;
using Texter.Intefaces;

namespace Texter.Controls.Confirmer
{
    public class MessageBoxConfirmer : IConfirmer
    {
        public bool ConfirmYesNo(string message, string caption)
        {
            return WPFMessageBox.WPFMessageBox.Show(caption, message, WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes;
        }

        public bool ConfirmYesNo(string message, string caption, string innerMessage)
        {
            return WPFMessageBox.WPFMessageBox.Show(caption, message, innerMessage, WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question) == WPFMessageBoxResult.Yes;
        }


        public void ConfirmOK(string message, string caption)
        {
            WPFMessageBoxResult result = WPFMessageBox.WPFMessageBox.Show(caption, message, WPFMessageBoxImage.OK);
        }

        public void ConfirmStop(string message, string caption)
        {
            WPFMessageBoxResult result = WPFMessageBox.WPFMessageBox.Show(caption, message, WPFMessageBoxButtons.OK, WPFMessageBoxImage.Error);
        }

        public bool ConfirmYesNo(string message, string caption, string yesText, string noText)
        {
            var options = new Dictionary<WPFMessageBoxResult, string>
            {
                {WPFMessageBoxResult.Yes, yesText},
                {WPFMessageBoxResult.No, noText}
            };
            return WPFMessageBox.WPFMessageBox.Show(caption, message, WPFMessageBoxButtons.YesNo, WPFMessageBoxImage.Question, options) == WPFMessageBoxResult.Yes;
        }

        /// <summary>
        /// A megadott opciók közül enged választani
        /// </summary>
        /// <param name="message"></param>
        /// <param name="caption"></param>
        /// <param name="details"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns>0 alapú indexelés, null, ha X-elték</returns>
        public int? SelectOption(string message, string caption, string details, string first, string second, string third)
        {
            var options = new Dictionary<WPFMessageBoxResult, string>
            {
                {WPFMessageBoxResult.Yes, first},
                {WPFMessageBoxResult.No, second},
                {WPFMessageBoxResult.Cancel, third}
            };

            WPFMessageBoxResult result = WPFMessageBox.WPFMessageBox.Show(caption, message, details, WPFMessageBoxButtons.YesNoCancel, WPFMessageBoxImage.Question, options);
            switch (result)
            {
                case WPFMessageBoxResult.Yes:
                    return 0;
                case WPFMessageBoxResult.No:
                    return 1;
                case WPFMessageBoxResult.Cancel:
                    return 2;
                case WPFMessageBoxResult.Close:
                    return null;
                default:
                    throw new System.Exception($"Invalid enum argument: {(int)result}, type: {typeof (WPFMessageBoxResult)}");
            }
        }
    }
}
