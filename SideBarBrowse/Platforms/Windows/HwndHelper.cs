#if WINDOWS
using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace SideBarBrowse
{
    internal static class HwndHelper
    {
        public static IntPtr GetMainWindowHandle()
        {
            var win = Application.Current?.Windows?.Count > 0 ? Application.Current.Windows[0] : null;
            if (win?.Handler?.PlatformView is Microsoft.Maui.MauiWinUIWindow mauiWindow)
            {
                return WinRT.Interop.WindowNative.GetWindowHandle(mauiWindow);
            }
            return IntPtr.Zero;
        }
    }
}
#endif
