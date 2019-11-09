namespace Overseer
{
    class Pair
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pair() : this(0, 0) { }
        public Pair(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
