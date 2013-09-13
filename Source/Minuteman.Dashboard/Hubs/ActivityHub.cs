namespace Minuteman.Dashboard
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    [CLSCompliant(false)]
    public abstract class ActivityHub : Hub
    {
        private static readonly ConcurrentDictionary<string, HashSet<string>>
            EventsToClientsMappings =
                new ConcurrentDictionary<string, HashSet<string>>();

        private static readonly ConcurrentDictionary<string, HashSet<string>>
            ClientsToEventsMappings =
                new ConcurrentDictionary<string, HashSet<string>>();

        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>(new JsonConverter[] { new StringEnumConverter() }),
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly Func<Task<IEnumerable<string>>> getEventNames;
        private readonly Func<string, Task<IEnumerable<ActivityTimeframe>>> getTimeframes;

        protected ActivityHub(
            Func<Task<IEnumerable<string>>> getEventNames,
            Func<string, Task<IEnumerable<ActivityTimeframe>>> getTimeframes)
        {
            if (getEventNames == null)
            {
                throw new ArgumentNullException("getEventNames");
            }

            if (getTimeframes == null)
            {
                throw new ArgumentNullException("getTimeframes");
            }

            this.getEventNames = getEventNames;
            this.getTimeframes = getTimeframes;
        }

        public JsonSerializerSettings SerializerSettings
        {
            get
            {
                return serializerSettings;
            }
        }

        public async Task Subscribe(string eventName)
        {
            var currentClient = Context.ConnectionId;

            var clients = EventsToClientsMappings.GetOrAdd(
                eventName,
                new HashSet<string>());

            bool shouldSubscribe;

            lock (clients)
            {
                shouldSubscribe = clients.Count == 0;
                clients.Add(currentClient);
            }

            var events = ClientsToEventsMappings.GetOrAdd(
                currentClient,
                new HashSet<string>());

            lock (events)
            {
                events.Add(eventName);
            }

            if (shouldSubscribe)
            {
                await OnSubscribe(eventName);
            }
        }

        public async Task Unsubscribe(string eventName)
        {
            var currentClient = Context.ConnectionId;

            HashSet<string> clients;
            var shouldUnsubscribe = false;

            if (EventsToClientsMappings.TryGetValue(eventName, out clients))
            {
                lock (clients)
                {
                    clients.Remove(currentClient);
                    shouldUnsubscribe = clients.Count == 0;
                }
            }

            HashSet<string> events;

            if (ClientsToEventsMappings.TryGetValue(currentClient, out events))
            {
                lock (events)
                {
                    events.Remove(eventName);
                }
            }

            if (shouldUnsubscribe)
            {
                await OnUnsubscribe(eventName);
            }
        }

        public Task<IEnumerable<string>> EventNames()
        {
            return getEventNames();
        }

        public async Task<IEnumerable<string>> Timeframes(
            string eventName)
        {
            var timeframes = await getTimeframes(eventName);
            var result = timeframes.Select(tf => tf.ToString()).ToList();

            return result;
        }

        public override Task OnDisconnected()
        {
            var currentClient = Context.ConnectionId;
            HashSet<string> events;

            if (!ClientsToEventsMappings.TryRemove(currentClient, out events))
            {
                return base.OnDisconnected();
            }

            var unsubscribes = new HashSet<string>();

            lock (events)
            {
                foreach (var @event in events)
                {
                    HashSet<string> clients;

                    if (!EventsToClientsMappings.TryGetValue(
                        @event,
                        out clients))
                    {
                        continue;
                    }

                    lock (clients)
                    {
                        if (!clients.Remove(currentClient))
                        {
                            continue;
                        }

                        if (clients.Count == 0)
                        {
                            unsubscribes.Add(@event);
                        }
                    }
                }
            }

            return unsubscribes.Count == 0 ?
                base.OnDisconnected() :
                Task.WhenAll(
                    unsubscribes.Select(OnUnsubscribe));
        }

        protected abstract Task OnSubscribe(string eventName);

        protected abstract Task OnUnsubscribe(string eventName);
    }
}