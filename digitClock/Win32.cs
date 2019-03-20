using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace digitClock
{
    class Win32
    {
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(int hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(int hwnd, int nIndex);
    }
}
