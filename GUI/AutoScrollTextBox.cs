using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MCForge.Gui
{
    public class AutoScrollTextBox : System.Windows.Forms.TextBox
    {
        // Constants for extern calls to various scrollbar functions
        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_BOTTOM = 7;
        private const int SB_OFFSET = 13;

        delegate void LogDelegate(string message);

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        private static extern bool PostMessageA(IntPtr hWnd, int nBar, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        public void AppendTextAndScroll(string text)
        {
            bool bottomFlag = false;
            int VSmin;
            int VSmax;
            int sbOffset;
            int savedVpos;
            // Make sure this is done in the UI thread
            if (this.InvokeRequired)
            {
                LogDelegate d = new LogDelegate(AppendTextAndScroll);
                this.Invoke(d, new object[] { text, true });
            }
            else
            {
                // Win32 magic to keep the textbox scrolling to the newest append to the textbox unless
                // the user has moved the scrollbox up
                sbOffset = (int)((this.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) / (this.Font.Height));
                savedVpos = GetScrollPos(this.Handle, SB_VERT);
                GetScrollRange(this.Handle, SB_VERT, out VSmin, out VSmax);
                if (savedVpos >= (VSmax - sbOffset - 1))
                    bottomFlag = true;
                this.AppendText(text + Environment.NewLine);
                if (bottomFlag)
                {
                    GetScrollRange(this.Handle, SB_VERT, out VSmin, out VSmax);
                    savedVpos = VSmax - sbOffset;
                    bottomFlag = false;
                }
                SetScrollPos(this.Handle, SB_VERT, savedVpos, true);
                PostMessageA(this.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * savedVpos, 0);
            }

        }
    }
}
