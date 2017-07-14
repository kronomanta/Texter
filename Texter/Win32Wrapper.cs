using System;
using System.Runtime.InteropServices;

namespace Texter
{
    class Win32Wrapper
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        /// <summary>
        /// Send the content of the clipboard to the active window. This app must be minimized before using this method
        /// </summary>
        /// <param name="text"></param>
        public static async System.Threading.Tasks.Task<IntPtr> PasteText(string text, bool keepOnClipboard)
        {
            if (string.IsNullOrWhiteSpace(text)) return IntPtr.Zero;
            
            IntPtr hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return IntPtr.Zero;

            string clipboard = System.Windows.Clipboard.GetText();
            System.Windows.Clipboard.SetDataObject(text);

            System.Windows.Forms.SendKeys.SendWait("^v");
            await System.Threading.Tasks.Task.Run(() => System.Threading.Thread.Sleep(100));

            if (!keepOnClipboard)
                System.Windows.Clipboard.SetDataObject(clipboard);

            return hWnd;
        }

        public static void ResizeWindow(IntPtr hWnd, ResizeDirection direction)
        {
            SendMessage(hWnd, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
        }

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }
    }
}
