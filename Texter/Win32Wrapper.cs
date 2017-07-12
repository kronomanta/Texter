using System;
using System.Runtime.InteropServices;

namespace Texter
{
    class Win32Wrapper
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        /// <summary>
        /// Send the content of the clipboard to the active window. This app must be minimized before using this method
        /// </summary>
        /// <param name="text"></param>
        public static IntPtr PasteText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return IntPtr.Zero;
            
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return IntPtr.Zero;

            string clipboard = System.Windows.Clipboard.GetText();
            System.Windows.Clipboard.SetDataObject(text);

            System.Windows.Forms.SendKeys.SendWait("^v");
            System.Windows.Clipboard.SetDataObject(clipboard);

            return hWnd;
        }
    }
}
