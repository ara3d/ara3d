using System;
using System.Collections.Concurrent;
using System.Linq;
using Ara3D.Utils;

namespace Ara3D.Services
{
    /// <summary>
    /// Event types should derive from this class. An event is a data packet
    /// broadcast for anyone listening.  
    /// </summary>
    public interface IEvent
    { }

    /// <summary>
    /// Listeners are notified when an event they have subscribed to has been sent.
    /// Filtering of events is done by the bus, only subscribed events are ever sent.
    /// Listeners inform when they are being disposed, so that they can be detached.  
    /// </summary>
    public interface ISubscriber<in T> : IDisposingNotifier
    {
        void OnEvent(T evt);
    }

    /// <summary>
    /// Used to subscribe to, and publish events between services.
    /// Events must have distinct type names.
    /// This decouples event publishers from subscribers.
    /// If a publisher is disposed, events are simply no longer published.
    /// If a subscriber is disposed, it is automatically unsubscribed.
    /// </summary>
    public interface IEventBus
    {
        void Publish<T>(T evt) where T : IEvent;
        void Unsubscribe<T>(ISubscriber<T> subscriber) where T : IEvent;
        void Subscribe<T>(ISubscriber<T> subscriber) where T : IEvent;
    }

    /// <summary>
    /// A very simple concurrent set used by the Event bus.
    /// Surprisingly absent from C#
    /// </summary>
    public class ConcurrentSet<T>
    {
        private readonly ConcurrentDictionary<T, bool> _dictionary = new ConcurrentDictionary<T, bool>();

        public void Add(T x)
            => _dictionary.TryAdd(x, true);

        public void Remove(T x)
            => _dictionary.TryRemove(x, out _);

        public T[] Values 
            => _dictionary.Keys.ToArray();
    }

    /// <summary>
    /// <inheritdoc cref="IEventBus"/>
    /// </summary>
    public class EventBus : IEventBus
    {
        public ConcurrentDictionary<Type, ConcurrentSet<object>> Subscribers = new ConcurrentDictionary<Type, ConcurrentSet<object>>();
        
        public void Publish<T>(T evt) where T: IEvent
        {
            if (!Subscribers.TryGetValue(typeof(T), out var subscribers)) 
                return;
            foreach (var s in subscribers.Values.Cast<ISubscriber<T>>())
                s.OnEvent(evt);
        }

        public void Subscribe<T>(ISubscriber<T> subscriber) where T : IEvent
        {
            subscriber.Disposing += (_sender, _args) => Unsubscribe(subscriber);
            var subscribers = Subscribers.GetOrAdd(typeof(T), _ => new ConcurrentSet<object>());
            subscribers.Add(subscriber);
        }

        public void Unsubscribe<T>(ISubscriber<T> subscriber) where T : IEvent
        {
            if (!Subscribers.TryGetValue(typeof(T), out var subscribers))
                return;
            subscribers.Remove(subscriber);
        }

        public void Clear()
        {
            Subscribers.Clear();
        }
    }
}
