using System.Collections.Generic;

namespace ppl.SimpleEventSystem
{
    public static class EventSystem<T>
    {
        public delegate void EventHandler(EventPayload<T> args);
        internal static Dictionary<string, Dictionary<IEventBindable, EventHandler>> _Listeners = new Dictionary<string, Dictionary<IEventBindable, EventHandler>>();
    }

    public static class EventSystemExtensions
    {
        public static void ListenToEvent<T>(this IEventBindable listener, string eventName, EventSystem<T>.EventHandler eventHandler)
        {
            if(!EventSystem<T>._Listeners.ContainsKey(eventName))
                EventSystem<T>._Listeners[eventName] = new Dictionary<IEventBindable, EventSystem<T>.EventHandler>();
            
            EventSystem<T>._Listeners[eventName][listener] = eventHandler;
        }

        public static void EmitEvent<T>(this IEventBindable emiter, string eventName, T data)
        {
            if (EventSystem<T>._Listeners.TryGetValue(eventName, out var listener))
            {
                foreach (var listeners in listener.Values)
                {
                    listeners(new EventPayload<T> { Args = data });
                }
            }
        }
        
        public static void StopListenForEvent<T>(this IEventBindable listener, string eventName)
        {
            if (EventSystem<T>._Listeners.ContainsKey(eventName))
            {
                if (EventSystem<T>._Listeners[eventName].ContainsKey(listener))
                {
                    EventSystem<T>._Listeners[eventName].Remove(listener);
                    if (EventSystem<T>._Listeners[eventName].Count == 0)
                    {
                        EventSystem<T>._Listeners.Remove(eventName);
                    }
                }
            }
        }
    }
}
