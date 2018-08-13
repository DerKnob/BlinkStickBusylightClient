namespace BlinkStickBusylightClient
{
    class BlinkStickState
    {
        public enum STATE { SET, MORPH, PULSE, BLINK };

        // current sate
        public STATE State { get; set; }
        public string Color { get; set; }
        public int Duration { get; set; }
        public int ThreadSleep { get; set; }
        public int Steps { get; set; }
        public int Delay { get; set; }

        public BlinkStickState()
        {
            State = STATE.SET;
            Color = "#000000";
            Duration = 0;
            ThreadSleep = 0;
            Steps = 0;
            Delay = 0;
        }
    }
}
