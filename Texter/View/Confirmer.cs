using System.Windows.Forms;
using Texter.Intefaces;

namespace Texter.View
{
    public class Confirmer : IConfirmer
    {
        public void ConfirmOK(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ConfirmStop(string error, string caption)
        {
            MessageBox.Show(error, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool ConfirmYesNo(string message, string caption)
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
