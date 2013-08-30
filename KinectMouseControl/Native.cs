using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KinectMouseControl
{
    static class Native
    {
        [DllImport("User32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
    }
}
