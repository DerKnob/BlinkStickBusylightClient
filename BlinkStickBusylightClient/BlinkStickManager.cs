using System;
using System.Text;
using System.Threading;
using BlinkStickDotNet;

namespace BlinkStickBusylightClient
{
    class BlinkStickManager
    {
        static private BlinkStickManager INSTANCE = null;
        static private Object lockGlobalAccess = new Object();

        private bool cancelThread = false;
        private bool isThreadRunning = false;
        private Object lockConcurrentModification = new Object();

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

        private BlinkStick GetBlinkStick()
        {
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
                    BlinkStick stick = BlinkStick.FindFirst();

                    if (stick != null)
                    {
                        isThreadRunning = true;
                        cancelThread = false;
                    }

                    return stick;
                }
            }
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

        public String GetDeviceInformation()
        {
            // output of the BlinkStick infos
            StringBuilder temp = new StringBuilder();
            try
            {
                BlinkStick device = GetBlinkStick();

                if (device == null)
                {
                    temp.Append("No BlinkStick connected");
                }
                else
                {
                    //Open the device
                    if (device.OpenDevice())
                    {
                        byte cr;
                        byte cg;
                        byte cb;

                        device.GetColor(out cr, out cg, out cb);

                        temp.Append("Serial:       " + device.Meta.Serial);
                        temp.Append("\n");
                        temp.Append(String.Format("    Device color: #{0:X2}{1:X2}{2:X2}", cr, cg, cb));
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

        private BlinkStickManager()
        {

        }

        /****************************************************/

        internal void SetAvailable()
        { 
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.Morph("green");

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void TurnOff()
        {
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.TurnOff();

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void SetDoNotDisturb()
        {
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.Morph("red");

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void SetBusy()
        {
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.Morph("yellow");

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void SetColor(string color)
        {
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.SetColor(color);

                        // cleanup
                        CloseDevice(device);
                    }
                }
                    catch (Exception)
                {
                }
            }).Start();
        }

        internal void MorphColor(string color)
        {
            new Thread(() =>
            {
                try
                {
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        device.Morph(color);

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
                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                    
                            while (cancelThread == false)
                            {
                                device.Pulse(color, repeates, duration, steps);

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

                    BlinkStick device = GetBlinkStick();

                    if (device == null)
                        return;

                    //Open the device
                    if (device.OpenDevice())
                    {
                        while (cancelThread == false)
                        {
                            device.Blink(color, repeates, delay);

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
