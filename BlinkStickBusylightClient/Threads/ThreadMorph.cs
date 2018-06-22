using BlinkStickDotNet;
using System.Threading;

namespace BlinkStickBusylightClient.Theads
{
    public class ThreadMorph
    {
        protected Thread thread = null;

        protected BlinkStick device;

        protected byte index;
        protected string color;
        protected int repeates;
        protected int duration;
        protected int steps;

        public ThreadMorph(BlinkStick device, byte index, string color, int repeates, int duration, int steps)
        {
            this.index = index;
            this.color = color;
            this.repeates = repeates;
            this.duration = duration;
            this.steps = steps;
            this.device = device;
        }

        public void Start()
        {
            thread = new Thread(() =>
            {
                device.Pulse(0, index, color, repeates, duration, steps);
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