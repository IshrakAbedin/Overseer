namespace Overseer
{
    class Interface
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Connected { get; set; }
        public string SSIDName { get; set; }
        public Interface() : this("", "", false, "") { }

        public Interface(string name, string description, bool connected, string sSIDName)
        {
            Name = name;
            Description = description;
            Connected = connected;
            SSIDName = sSIDName;
        }

    }
}