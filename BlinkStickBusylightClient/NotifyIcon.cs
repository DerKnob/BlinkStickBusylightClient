using BlinkStickBusylightClient.Helper;
using Microsoft.Win32;
using System;

namespace BlinkStickBusylightClient
{
    class NotifyIcon
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.MenuItem itemAutostart;

        private RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public NotifyIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = Properties.Resources.trayicon;
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new EventHandler(notifyIconDoubleClick);

            System.Windows.Forms.ContextMenu notifyItemContextMenu = new System.Windows.Forms.ContextMenu();

            // create context menu
            /*
            notifyItemContextMenu.MenuItems.Add("&Available", new EventHandler(SetStatusAvailable));
            notifyItemContextMenu.MenuItems.Add("&Busy", new EventHandler(SetStatusBusy));
            notifyItemContextMenu.MenuItems.Add("&Do Not Disturb", new EventHandler(SetStatusDoNotDisturb));
            notifyItemContextMenu.MenuItems.Add("&Turn Off", new EventHandler(SetStatusTurnOff));
            notifyItemContextMenu.MenuItems.Add("-");
            */
            notifyItemContextMenu.MenuItems.Add("About BlinkStick Busylight Client", new EventHandler(About));
            notifyItemContextMenu.MenuItems.Add("-");
            itemAutostart = notifyItemContextMenu.MenuItems.Add("&Autostart BlinkStick Busylight Client", new EventHandler(Autostart));
            notifyItemContextMenu.MenuItems.Add("-");
            notifyItemContextMenu.MenuItems.Add("&Exit", new EventHandler(Close));
            
            notifyIcon.ContextMenu = notifyItemContextMenu;

            // load settings
            if (rkApp.GetValue(EnvironmentUtils.getApplicationName()) == null)
            {
                // The value doesn't exist, the application is not set to run at startup
                itemAutostart.Checked = false;
            }
            else
            {
                // The value exists, the application is set to run at startup
                itemAutostart.Checked = true;
            }
        }

        public void Shutdown()
        {
            notifyIcon.Visible = false;
        }

        private void notifyIconDoubleClick(Object sender, EventArgs e)
        {
            MainWindow.GetInstance().Visibility = System.Windows.Visibility.Visible;

        }

        private void Close(Object sender, EventArgs e)
        {
            Shutdown();

            if (System.Windows.Forms.Application.MessageLoop)
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                System.Environment.Exit(1);
            }
        }

        /******************************************/
        // Context Menu Items
        /******************************************/

        private void About(Object sender, EventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void SetStatusBusy(Object sender, EventArgs e)
        {
            BlinkStickManager.GetInstance().SetBusy();
        }

        private void SetStatusAvailable(Object sender, EventArgs e)
        {
            BlinkStickManager.GetInstance().SetAvailable();
        }

        private void SetStatusDoNotDisturb(Object sender, EventArgs e)
        {
            BlinkStickManager.GetInstance().SetDoNotDisturb();
        }

        private void SetStatusTurnOff(Object sender, EventArgs e)
        {
            BlinkStickManager.GetInstance().TurnOff();
        }

        private void Autostart(Object sender, EventArgs e)
        {
            if (itemAutostart.Checked == false)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue(EnvironmentUtils.getApplicationName(), EnvironmentUtils.GetProgramDirectory() + EnvironmentUtils.getExecuteexecutableFile());
                itemAutostart.Checked = true;
            }
            else
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue(EnvironmentUtils.getApplicationName(), false);
                itemAutostart.Checked = false;
            }
        }
    }
}
