namespace Iwan.Server.Constants
{
    public static class ServerState
    {
        private static readonly object _lock = new();

        private static bool _workingOnProducts = false;

        public static bool WorkingOnProducts
        {
            get
            {
                lock(_lock) return _workingOnProducts;
            }

            set
            {
                lock (_lock) _workingOnProducts = value;
            }
        }
    }
}
