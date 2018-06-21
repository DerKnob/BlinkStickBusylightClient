using System.Windows;
using System.Windows.Input;

namespace BlinkStickBusylightClient
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        //KeyboardListener KListener = new KeyboardListener();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //KListener.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
        }

        void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            if ((Keyboard.IsKeyDown(Key.LeftShift) == false) || (Keyboard.IsKeyDown(Key.LeftCtrl) == false))
            {
                return;
            }

            if (args.Key.Equals(Key.F9))
            {
                BlinkStickManager.GetInstance().SetAvailable();
            }
            else if (args.Key.Equals(Key.F10))
            {
                BlinkStickManager.GetInstance().SetBusy();
            }
            else if (args.Key.Equals(Key.F11))
            {
                BlinkStickManager.GetInstance().SetDoNotDisturb();
            }
            else if (args.Key.Equals(Key.F12))
            {
                BlinkStickManager.GetInstance().TurnOff();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //KListener.Dispose();
        }
    }
}
