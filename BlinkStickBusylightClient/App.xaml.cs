using System.Windows;
using System.Windows.Input;

namespace BlinkStickBusylightClient
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        private GlobalKeyboardHook globalKeyboardHook;
        private bool keyPressedLeftCTRL;
        private bool keyPressedLShift;
        private bool keyPressedF9;
        private bool keyPressedF10;
        private bool keyPressedF11;
        private bool keyPressedF12;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.OnKeyPressed += keyboardHookListner_OnKeyPressed;
            globalKeyboardHook.OnKeyUnpressed += keyboardHookListner_OnKeyUnPressed;
            globalKeyboardHook.HookKeyboard();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Close();
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            Close();
        }

        public void Close()
        {
            BlinkStickManager.GetInstance().TurnOff(true);

            globalKeyboardHook.UnHookKeyboard();
        }

        private void keyboardHookListner_OnKeyPressed(object sender, System.Windows.Forms.Keys e)
        {
            if (e == System.Windows.Forms.Keys.LControlKey)
            {
                keyPressedLeftCTRL = true;
            }
            else if (e == System.Windows.Forms.Keys.LShiftKey)
            {
                keyPressedLShift = true;
            }
            else if (e == System.Windows.Forms.Keys.F9)
            {
                keyPressedF9 = true;
            }
            else if (e == System.Windows.Forms.Keys.F10)
            {
                keyPressedF10 = true;
            }
            else if (e == System.Windows.Forms.Keys.F11)
            {
                keyPressedF11 = true;
            }
            else if (e == System.Windows.Forms.Keys.F12)
            {
                keyPressedF12 = true;
            }

            FireShortcut();
        }

        void keyboardHookListner_OnKeyUnPressed(object sender, System.Windows.Forms.Keys e)
        {
            if (e == System.Windows.Forms.Keys.LControlKey)
            {
                keyPressedLeftCTRL = false;
            }
            else if (e == System.Windows.Forms.Keys.LShiftKey)
            {
                keyPressedLShift = false;
            }
            else if (e == System.Windows.Forms.Keys.F9)
            {
                keyPressedF9 = false;
            }
            else if (e == System.Windows.Forms.Keys.F10)
            {
                keyPressedF10 = false;
            }
            else if (e == System.Windows.Forms.Keys.F11)
            {
                keyPressedF11 = false;
            }
            else if (e == System.Windows.Forms.Keys.F12)
            {
                keyPressedF12 = false;
            }
        }

        private void FireShortcut()
        {
            if (keyPressedLeftCTRL && keyPressedLShift)
            {
                if (keyPressedF9)
                {
                    BlinkStickManager.GetInstance().SetAvailable();
                }
                else if (keyPressedF10)
                {
                    BlinkStickManager.GetInstance().SetBusy();
                }
                else if (keyPressedF11)
                {
                    BlinkStickManager.GetInstance().SetDoNotDisturb();
                }
                else if (keyPressedF12)
                {
                    BlinkStickManager.GetInstance().SetPhoneCall();
                }
            }
        }
    }
}
