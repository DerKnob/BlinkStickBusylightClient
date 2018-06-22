using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BlinkStickBusylightClient.Theads;
using BlinkStickDotNet;
using BlinkStickDotNet.Usb;

namespace BlinkStickBusylightClient
{
    class BlinkStickManager
    {
        static private BlinkStickManager INSTANCE = null;
        static private Object lockGlobalAccess = new Object();
        static private Object lockConcurrentModification = new Object();
        private static int numberOfLeds = 8;

        private bool cancelThread = false;
        private bool isThreadRunning = false;
        private UsbMonitor monitor;
        private BlinkStick device = null;
        private String deviceInfo = "No BlinkStick connected";

        private List<BlinkStickActionListener> blinkStickActionListener = new List<BlinkStickActionListener>();

        private BlinkStickManager()
        {
            monitor = new UsbMonitor();

            //Attach to connected event
            monitor.Connected += (object sender, DeviceModifiedArgs e) => {
                BlinkStick blinkStick = GetBlinkStick();

                if (blinkStick == null)
                    return;

                InitDevice(blinkStick);                
            };

            //Attach to disconnected event
            monitor.Disconnected += (object sender, DeviceModifiedArgs e) => {
                // reset
                device = null;

                deviceInfo = GetDeviceInformationInternal();

                // Send notification to subscribers
                foreach (BlinkStickActionListener listener in blinkStickActionListener)
                {
                    listener.OnDisconnect();
                }
            };

            //Start monitoring
            monitor.Start();

            // check if BlinkStick is already connected
            BlinkStick fistDevice = BlinkStick.FindFirst();

            if (fistDevice == null)
                return;

            InitDevice(fistDevice);
        }

        private void InitDevice(BlinkStick newDevice)
        {
            device = newDevice;

            OpenDevice(device);
            device.SetMode(2);
            CloseDevice(device);

            deviceInfo = GetDeviceInformationInternal();

            // Send notification to subscribers
            foreach (BlinkStickActionListener listener in blinkStickActionListener)
            {
                listener.OnConnect();
            }
        }

        static public BlinkStickManager GetInstance()
        {
            lock (lockGlobalAccess)
            {
                if (INSTANCE == null)
                {
                    INSTANCE = new BlinkStickManager();
                }

                return INSTANCE;
            }
        }

        /****************************************************/
        /****************************************************/

        private BlinkStick GetBlinkStick()
        {
            BlinkStick stick = BlinkStick.FindFirst();

            return stick;
        }

        private void CloseDevice(BlinkStick device)
        {
            lock (lockConcurrentModification)
            {
                device.CloseDevice();

                isThreadRunning = false;
                cancelThread = false;
            }
        }

        private bool OpenDevice(BlinkStick device)
        {
            if (device == null)
                return false;

            bool result = false;

            lock (lockGlobalAccess)
            {
                lock (lockConcurrentModification)
                {
                    // is alreads a thread running?
                    if (isThreadRunning)
                    {
                        // set the cancel flag
                        cancelThread = true;
                    }
                }

                // and wait for cancel
                while (isThreadRunning)
                {
                    Thread.Sleep(10);
                }

                lock (lockConcurrentModification)
                {
                    if (device != null)
                    {
                        isThreadRunning = true;
                        cancelThread = false;

                        result = device.OpenDevice();
                    }
                }
            }
            return result;
        }

        /****************************************************/

        public String GetDeviceInformation()
        {
            return deviceInfo;
        }

        private String GetDeviceInformationInternal()
        {
            // output of the BlinkStick infos
            StringBuilder temp = new StringBuilder();
            try
            {
                if (device == null)
                {
                    temp.Append("No BlinkStick connected");
                }
                else
                {
                    //Open the device
                    if (OpenDevice(device))
                    {
                        temp.Append("Serial:       " + device.Meta.Serial);
                        temp.Append("\n");
                        temp.Append("    Manufacturer:  " + device.Meta.Manufacturer);
                        temp.Append("\n");
                        temp.Append("    Product Name:  " + device.Meta.ProductName);
                        temp.Append("\n");
                        temp.Append("    Version Major: " + device.Meta.VersionMajor);
                        temp.Append("\n");
                        temp.Append("    Version Minor: " + device.Meta.VersionMinor);
                        temp.Append("\n");
                        temp.Append("    InfoBlock1:    " + device.Meta.InfoBlock1);
                        temp.Append("\n");
                        temp.Append("    InfoBlock2:    " + device.Meta.InfoBlock2);

                        // cleanup
                        CloseDevice(device);
                    }
                    else
                    {
                        temp.Append("Could not open BlinkStick connection");
                    }
                }
            }
            catch (Exception ex)
            {
                temp.Append(ex.Message);
            }

            return temp.ToString();
        }

        /****************************************************/

        public void AddListener(BlinkStickActionListener listener)
        {
            blinkStickActionListener.Add(listener);

            // Send notification to new subscriber if already connected
            if (device != null)
                listener.OnConnect();
        }

        public void RemoveListener(BlinkStickActionListener listener)
        {
            blinkStickActionListener.Remove(listener);
        }

        /****************************************************/

        internal void TurnOff()
        {
            SetColor("#000000");
        }

        internal void SetAvailable()
        {
            MorphColor("#00FF00");
        }

        internal void SetBusy()
        {
            MorphColor("#FFAA00");
        }

        internal void SetDoNotDisturb()
        {
            MorphColor("#FF0000");
        }

        internal void SetPhoneCall()
        {
            PulseColor("#FF0000");
        }

        /****************************************************/

        internal void SetColor(string color)
        {
            new Thread(() =>
            {
                try
                {
                    //Open the device
                    if (OpenDevice(device))
                    {
                        for (byte i = 0; i < numberOfLeds; i++)
                        {
                            device.SetColor(0, i, color);
                        }

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void MorphColor(string color, int duration = 125)
        {
            new Thread(() =>
            {
                try
                {
                    //Open the device
                    if (OpenDevice(device))
                    {
                        for (byte i = 0; i < numberOfLeds; i++)
                        {
                            device.Morph(0, i, color, duration / numberOfLeds);
                        }

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void PulseColor(string color, int duration = 1000, int steps = 50, int threadSleep = 100)
        {
            new Thread(() =>
            {
                int repeates = 1;

                try
                {
                    //Open the device
                    if (OpenDevice(device))
                    {
                        while (cancelThread == false)
                        {
                            List<ThreadMorph> threadList = new List<ThreadMorph>();
                            for (byte i = 0; i < numberOfLeds; i++)
                            {
                                ThreadMorph thread = new ThreadMorph(device, i, color, repeates, duration, steps);
                                threadList.Add(thread);
                                thread.Start();
                            }

                            // wait for all threads to finish
                            foreach (ThreadMorph thread in threadList)
                            {
                                while (thread.IsRunning())
                                    Thread.Sleep(10);
                            }

                            Thread.Sleep(threadSleep);
                        }

                        // cleanup
                        CloseDevice(device);

                    }
                }
                catch (Exception)
                {
                }
            }).Start();
        }

        internal void BlinkColor(string color, int delay = 500, int threadSleep = 100)
        {
            new Thread(() =>
            {
                int repeates = 1;

                try
                {
                    //Open the device
                    if (OpenDevice(device))
                    {
                        while (cancelThread == false)
                        {
                            List<ThreadBlink> threadList = new List<ThreadBlink>();
                            for (byte i = 0; i < numberOfLeds; i++)
                            {
                                ThreadBlink thread = new ThreadBlink(device, i, color, repeates, delay);
                                threadList.Add(thread);
                                thread.Start();
                            }

                            // wait for all threads to finish
                            foreach (ThreadBlink thread in threadList)
                            {
                                while (thread.IsRunning())
                                    Thread.Sleep(10);
                            }

                            Thread.Sleep(threadSleep);
                        }

                        // cleanup
                        CloseDevice(device);
                    }
                }
                catch (Exception)
                {
                }
            }).Start();
        }
    }
}
