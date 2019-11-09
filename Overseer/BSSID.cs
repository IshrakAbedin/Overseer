namespace Overseer
{
    class BSSID
    {
        public string MAC { get; set; }
        public ushort Signal { get; set; }
        public string RadioType { get; set; }
        public ushort Channel { get; set; }
        public string BasicRates { get; set; }
        public string OtherRates { get; set; }

        public BSSID() : this("", 255, "", 255, "", "") { }

        public BSSID(string mAC, ushort signal, string radioType, ushort channel, string basicRates, string otherRates)
        {
            MAC = mAC;
            Signal = signal;
            RadioType = radioType;
            Channel = channel;
            BasicRates = basicRates;
            OtherRates = otherRates;
        }

    }
}