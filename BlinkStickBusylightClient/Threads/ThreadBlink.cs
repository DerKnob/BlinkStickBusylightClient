using BlinkStickDotNet;
using System.Threading;

namespace BlinkStickBusylightClient.Theads
{
    public class ThreadBlink
    {
        protected Thread thread = null;

        protected BlinkStick device;

        protected byte index;
        protected string color;
        protected int repeates;
        protected int delay;

        public ThreadBlink(BlinkStick device, byte index, string color, int repeates, int delay)
        {
            this.index = index;
            this.color = color;
            this.repeates = repeates;
            this.delay = delay;
            this.device = device;
        }

        public void Start()
        {
            thread = new Thread(() =>
            {
                device.Blink(color, repeates, delay);
            });
            thread.Start();
        }

        public bool IsRunning()
        {
            if (thread == null)
                return false;

            if (thread.ThreadState == ThreadState.Running)
                return true;

            return false;
        }
    }
}
