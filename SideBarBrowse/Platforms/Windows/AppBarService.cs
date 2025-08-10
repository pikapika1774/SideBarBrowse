#if WINDOWS
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SideBarBrowse
{
    internal enum AppBarEdge
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }

    internal static class AppBarService
    {
        private const uint ABM_NEW = 0x00000000;
        private const uint ABM_REMOVE = 0x00000001;
        private const uint ABM_QUERYPOS = 0x00000002;
        private const uint ABM_SETPOS = 0x00000003;

        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        private struct APPBARDATA
        {
            public uint cbSize;
            public IntPtr hWnd;
            public uint uCallbackMessage;
            public uint uEdge;
            public RECT rc;
            public int lParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left; Top = top; Right = right; Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public uint cbSize;
            public RECT rcMonitor; // ディスプレイ全体
            public RECT rcWork;    // タスクバー等を除いた作業領域
            public uint dwFlags;
        }

        [DllImport("shell32.dll")]
        private static extern uint SHAppBarMessage(uint dwMessage, ref APPBARDATA pData);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        private static IntPtr _hWnd = IntPtr.Zero;
        private static bool _registered = false;
        private static uint _callbackMsg = 0;

        public static void RegisterAppBar(IntPtr hWnd)
        {
            if (_registered) return;

            _hWnd = hWnd;
            var msgName = "SideBarBrowse_AppBar_" + Process.GetCurrentProcess().Id;
            _callbackMsg = RegisterWindowMessage(msgName);

            var abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf<APPBARDATA>(),
                hWnd = _hWnd,
                uCallbackMessage = _callbackMsg
            };
            SHAppBarMessage(ABM_NEW, ref abd);
            _registered = true;
        }

        public static void Dock(AppBarEdge edge, double widthRatio = 0.25)
        {
            if (!_registered || _hWnd == IntPtr.Zero) return;

            // ウィンドウが属するモニタの矩形を取得
            var hMon = MonitorFromWindow(_hWnd, MONITOR_DEFAULTTONEAREST);
            var mi = new MONITORINFO { cbSize = (uint)Marshal.SizeOf<MONITORINFO>() };
            if (!GetMonitorInfo(hMon, ref mi)) return;

            int monitorW = mi.rcMonitor.Right - mi.rcMonitor.Left;
            int monitorH = mi.rcMonitor.Bottom - mi.rcMonitor.Top;

            int desiredWidth = Math.Max(200, (int)(monitorW * widthRatio));

            RECT rc;
            if (edge == AppBarEdge.Left)
            {
                rc = new RECT(mi.rcMonitor.Left, mi.rcMonitor.Top,
                              mi.rcMonitor.Left + desiredWidth, mi.rcMonitor.Bottom);
            }
            else // Right
            {
                rc = new RECT(mi.rcMonitor.Right - desiredWidth, mi.rcMonitor.Top,
                              mi.rcMonitor.Right, mi.rcMonitor.Bottom);
            }

            var abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf<APPBARDATA>(),
                hWnd = _hWnd,
                uEdge = (uint)edge,
                rc = rc
            };

            // シェルに問い合わせ/調整
            SHAppBarMessage(ABM_QUERYPOS, ref abd);

            // 幅を確定
            if (edge == AppBarEdge.Left)
                abd.rc.Right = abd.rc.Left + desiredWidth;
            else
                abd.rc.Left = abd.rc.Right - desiredWidth;

            // 位置確定
            SHAppBarMessage(ABM_SETPOS, ref abd);

            int x = abd.rc.Left;
            int y = abd.rc.Top;
            int w = abd.rc.Right - abd.rc.Left;
            int h = abd.rc.Bottom - abd.rc.Top;

            // ウィンドウを予約領域に合わせて移動・サイズ確定
            SetWindowPos(_hWnd, IntPtr.Zero, x, y, w, h,
                SWP_NOZORDER | SWP_NOACTIVATE | SWP_SHOWWINDOW);
        }

        public static void RemoveAppBar()
        {
            if (!_registered || _hWnd == IntPtr.Zero) return;
            var abd = new APPBARDATA
            {
                cbSize = (uint)Marshal.SizeOf<APPBARDATA>(),
                hWnd = _hWnd
            };
            SHAppBarMessage(ABM_REMOVE, ref abd);
            _registered = false;
            _hWnd = IntPtr.Zero;
        }
    }
}
#endif
