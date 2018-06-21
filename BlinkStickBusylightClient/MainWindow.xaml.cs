using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BlinkStickBusylightClient
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static private MainWindow INSTANCE = null;

        public MainWindow()
        {
            InitializeComponent();

            INSTANCE = this;

            this.Visibility = Visibility.Hidden;
            NotifyIcon notifyIcon = new NotifyIcon();
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

        private void buttonAvailable_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetAvailable();
            this.Visibility = Visibility.Hidden;
        }

        private void buttonBusy_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetBusy();
            this.Visibility = Visibility.Hidden;
        }

        private void buttonDoNotDisturb_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().SetDoNotDisturb();
            this.Visibility = Visibility.Hidden;
        }

        private void buttonTurnOff_Click(object sender, RoutedEventArgs e)
        {
            BlinkStickManager.GetInstance().TurnOff();
            this.Visibility = Visibility.Hidden;
        }

        private void buttonAdvanced_Checked(object sender, RoutedEventArgs e)
        {
            gridAdvanced.Visibility = Visibility.Visible;
        }

        private void buttonAdvanced_Unchecked(object sender, RoutedEventArgs e)
        {
            gridAdvanced.Visibility = Visibility.Collapsed;
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
                BlinkStickManager.GetInstance().MorphColor(color);
            }
            else if (radioButtonMode3.IsChecked == true)
            {
                BlinkStickManager.GetInstance().BlinkColor(color, delay, sleep);
            }
            else if (radioButtonMode4.IsChecked == true)
            {
                BlinkStickManager.GetInstance().PulseColor(color, delay, sleep);
            }
        }

        private void radioButtonMode_Checked(object sender, RoutedEventArgs e)
        {
            if (textBoxDelay == null)
                return;

            if ((radioButtonMode1.IsChecked == true) || (radioButtonMode2.IsChecked == true))
            {
                gridAdditionalSettings.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridAdditionalSettings.Visibility = Visibility.Visible;
            }
        }

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
    }
}
