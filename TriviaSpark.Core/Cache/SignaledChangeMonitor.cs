using System.Collections.Concurrent;
using System.Globalization;
using System.Runtime.Caching;

namespace TriviaSpark.Core.Cache
{
    /// <summary>
    /// Cache change monitor that allows an app to fire a change notification
    /// to all associated cache items.
    /// </summary>
    public class SignaledChangeMonitor : ChangeMonitor
    {
        // Shared across all SignaledChangeMonitors in the AppDomain
        private static ConcurrentDictionary<string, EventHandler<SignaledChangeEventArgs>> ListenerLookup =
            new ConcurrentDictionary<string, EventHandler<SignaledChangeEventArgs>>();

        private string _name;
        private string _key;
        private string _uniqueId = Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);

        public override string UniqueId
        {
            get { return _uniqueId; }
        }

        public SignaledChangeMonitor(string key, string name)
        {
            _key = key;
            _name = name;
            // Register instance with the shared event
            ListenerLookup[_uniqueId] = OnSignalRaised;
            InitializationComplete();
        }


        public static void Signal(string name = null)
        {
            // Raise shared event to notify all subscribers
            foreach (var subscriber in ListenerLookup.ToList())
            {
                subscriber.Value?.Invoke(null, new SignaledChangeEventArgs(name));
            }
        }

        protected override void Dispose(bool disposing)
        {
            // Set delegate to null so it can't be accidentally called in Signal() while being disposed
            ListenerLookup[_uniqueId] = null;
            EventHandler<SignaledChangeEventArgs> outValue = null;
            ListenerLookup.TryRemove(_uniqueId, out outValue);
        }

        private void OnSignalRaised(object sender, SignaledChangeEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Name) || string.Compare(e.Name, _name, true) == 0)
            {
                // Cache objects are obligated to remove entry upon change notification.
                OnChanged(null);
            }
        }
    }
}