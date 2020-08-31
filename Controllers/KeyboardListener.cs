using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SupremeXE_UI
{
    class InterceptKeys
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public static IntPtr _hookID = IntPtr.Zero;
        public static LowLevelKeyboardProc _proc = HookCallback;
        public static Browser browser = null;
        public static Config config = null; 

        static InterceptKeys()
        {
            browser = Browser.GetInstance();
            config = Config.GetInstance();
        }

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if((System.Windows.Forms.Keys)vkCode == System.Windows.Forms.Keys.F1)
                {
                    if(browser.driver != null)
                    {
                        if (browser.driver.Url != config.Links.StoreMain)
                            browser.Homepage();
                    }
                }
                else if((System.Windows.Forms.Keys)vkCode == System.Windows.Forms.Keys.F2)
                {
                    if (browser.driver != null)
                    {
                        browser.AddRemoveItem();
                    }
                }
                else if((System.Windows.Forms.Keys)vkCode == System.Windows.Forms.Keys.F3)
                {
                    if (browser.driver != null)
                    {
                        if (browser.driver.Url != config.Links.StoreCart)
                            browser.Cart();
                    }
                }
                else if((System.Windows.Forms.Keys)vkCode == System.Windows.Forms.Keys.F4)
                {
                    if (browser.driver != null)
                    {
                        if (browser.driver.Url != config.Links.StoreCheckout)
                            browser.Checkout();
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}