namespace Texter.Intefaces
{
    public interface IConfirmer
    {
        void ConfirmOK(string message, string caption);
        void ConfirmStop(string error, string caption);
        bool ConfirmYesNo(string message, string caption);
    }
}
