using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KinectMouseControl
{
    static class MouseClicker
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private static void SetMouseState(int state)
        {
            int x = Cursor.Position.X;
            int y = Cursor.Position.Y;
            mouse_event(
                (uint)state,
                (uint)x,
                (uint)y,
                0, 0);
        }

        public static void SetMouseLeftButtonDown()
        {
            SetMouseState(MOUSEEVENTF_LEFTDOWN);
        }

        public static void SetMouseLeftButtonUp()
        {
            SetMouseState(MOUSEEVENTF_LEFTUP);
        }
    }
}
