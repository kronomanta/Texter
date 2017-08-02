namespace Texter.Intefaces
{
    public interface IConfirmer
    {
        void ConfirmOK(string message, string caption);
        void ConfirmStop(string message, string caption);
        void ConfirmStop(string message);

        bool ConfirmYesNo(string message, string caption);
        bool ConfirmYesNo(string message, string caption, string yesText, string noText);
        bool ConfirmYesNo(string message, string caption, string innerMessage);

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
        int? SelectOption(string message, string caption, string details, string first, string second, string third);
    }
}
