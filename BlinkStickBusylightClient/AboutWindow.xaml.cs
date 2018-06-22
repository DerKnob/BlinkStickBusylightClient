using BlinkStickBusylightClient.Helper;
using System;
using System.Windows;

namespace BlinkStickBusylightClient
{
    /// <summary>
    /// Interaktionslogik für AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            this.Title = this.Title + " " + EnvironmentUtils.getApplicationName();

            string currentVersion = EnvironmentUtils.GetCurrentVersion();
            textBlockVersion.Text = "BlinkStick Busylight Client" + " v." + currentVersion;

            textBoxTeam.Text = "Christian Knobloch" + Environment.NewLine + "";

            String tempText = BlinkStickManager.GetInstance().GetDeviceInformation();
            tempText = tempText.Replace("\n", Environment.NewLine);
            textBoxDeviceInfo.Text = BlinkStickManager.GetInstance().GetDeviceInformation();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
