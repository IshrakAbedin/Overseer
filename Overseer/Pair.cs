namespace Overseer
{
    class Pair
    {
        public int Channel { get; set; }
        public int Strength { get; set; }

        public Pair() : this(0, 0) { }
        public Pair(int channel, int strength)
        {
            Channel = channel;
            Strength = strength;
        }
    }
}
