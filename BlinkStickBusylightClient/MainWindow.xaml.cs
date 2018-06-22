using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BlinkStickBusylightClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, BlinkStickActionListener
    {
        static private MainWindow INSTANCE = null;

        public MainWindow()
        {
            InitializeComponent();

            INSTANCE = this;

            this.Visibility = Visibility.Hidden;
            NotifyIcon notifyIcon = new NotifyIcon();

            // init instance (disconnect / connect monitor) and add this window as listener
            BlinkStickManager.GetInstance().AddListener(this);

            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        static public MainWindow GetInstance()
        {
            return INSTANCE;
        }

        /****************************************************/
        /****************************************************/

        private void buttonAvailable_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetAvailable();

            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        private void buttonBusy_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetBusy();

            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        private void buttonDoNotDisturb_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetDoNotDisturb();
            
            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        private void buttonTurnOff_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().TurnOff();

            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        private void buttonPhoneCall_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetPhoneCall();

            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        /****************************************************/

        private void buttonAdvanced_Checked(object sender, RoutedEventArgs e)
        {
            gridAdvanced.Visibility = Visibility.Visible;
        }

        private void buttonAdvanced_Unchecked(object sender, RoutedEventArgs e)
        {
            gridAdvanced.Visibility = Visibility.Collapsed;
        }


        private void radioButtonMode_Checked(object sender, RoutedEventArgs e)
        {
            if (textBoxDelay == null)
                return;

            if ((radioButtonMode1.IsChecked == true))
            {
                gridAdditionalSettings.Visibility = Visibility.Collapsed;
            }
            else if (radioButtonMode2.IsChecked == true)
            {
                gridAdditionalSettings.Visibility = Visibility.Visible;
                labelSleep.Visibility = Visibility.Collapsed;
                textBoxSleep.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridAdditionalSettings.Visibility = Visibility.Visible;
                labelSleep.Visibility = Visibility.Visible;
                textBoxSleep.Visibility = Visibility.Visible;
            }
        }

        private void buttonSetAdvanced_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxDelay.Text.Length == 0)
                textBoxDelay.Text = "0";

            if (textBoxSleep.Text.Length == 0)
                textBoxSleep.Text = "0";

            string color = colorPicker.SelectedColorText;
            int delay = Int32.Parse(textBoxDelay.Text);
            int sleep = Int32.Parse(textBoxSleep.Text);

            if (radioButtonMode1.IsChecked == true)
            {
                BlinkStickManager.GetInstance().SetColor(color);
            }
            else if (radioButtonMode2.IsChecked == true)
            {
                BlinkStickManager.GetInstance().MorphColor(color, delay);
            }
            else if (radioButtonMode3.IsChecked == true)
            {
                BlinkStickManager.GetInstance().BlinkColor(color, delay, sleep);
            }
            else if (radioButtonMode4.IsChecked == true)
            {
                BlinkStickManager.GetInstance().PulseColor(color, delay, sleep);
            }

            // hide main window
            this.Visibility = Visibility.Hidden;
        }

        /****************************************************/

        // only allow numbers
        private void textBox_NumberOnly(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        /****************************************************/
        /****************************************************/

        // BlinkStick Action Listener
        public void OnConnect()
        {
            BlinkStickManager.GetInstance().SetAvailable();
        }

        public void OnDisconnect()
        {
            
        }

        /****************************************************/

        static void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                BlinkStickManager.GetInstance().SetDoNotDisturb();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                BlinkStickManager.GetInstance().SetAvailable();
            }
        }
    }
}
