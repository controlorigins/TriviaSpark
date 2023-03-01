namespace TriviaSpark.Core.Cache
{
    public class SignaledChangeEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public SignaledChangeEventArgs(string name = null) { Name = name; }
    }
}