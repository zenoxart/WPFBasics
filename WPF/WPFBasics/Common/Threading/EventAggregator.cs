using System.Collections.Concurrent;

namespace WPFBasics.Common.Threading
{
    /// <summary>
    /// Ermöglicht die lose gekoppelte Kommunikation zwischen Komponenten über das Publish/Subscribe-Prinzip.
    /// Unterstützt starke und schwache Abonnements.
    /// </summary>
    public class EventAggregator
    {
        private readonly ConcurrentDictionary<Type, List<Delegate>> _subscribers = new();
        private readonly ConcurrentDictionary<Type, List<WeakReference>> _weakSubscribers = new();

        /// <summary>
        /// Abonniert Nachrichten eines bestimmten Typs.
        /// </summary>
        /// <typeparam name="T">Der Nachrichtentyp.</typeparam>
        /// <param name="handler">Die auszuführende Aktion beim Empfang.</param>
        /// <param name="useWeakReference">Ob das Abonnement schwach erfolgen soll (verhindert Memory-Leaks).</param>
        public void Subscribe<T>(Action<T> handler, bool useWeakReference = false)
        {
            if (useWeakReference)
            {
                List<WeakReference> list = _weakSubscribers.GetOrAdd(typeof(T), _ => []);
                lock (list)
                {
                    list.Add(new WeakReference(handler));
                }
            }
            else
            {
                List<Delegate> list = _subscribers.GetOrAdd(typeof(T), _ => []);
                lock (list)
                {
                    list.Add(handler);
                }
            }
        }

        /// <summary>
        /// Hebt das Abonnement für Nachrichten eines bestimmten Typs auf.
        /// </summary>
        /// <typeparam name="T">Der Nachrichtentyp.</typeparam>
        /// <param name="handler">Die abzumeldende Aktion.</param>
        /// <param name="useWeakReference">Ob das Abonnement schwach war.</param>
        public void Unsubscribe<T>(Action<T> handler, bool useWeakReference = false)
        {
            if (useWeakReference)
            {
                if (_weakSubscribers.TryGetValue(typeof(T), out List<WeakReference> list))
                {
                    lock (list)
                    {
                        list.RemoveAll(wr => wr.Target is Action<T> target && target == handler);
                    }
                }
            }
            else
            {
                if (_subscribers.TryGetValue(typeof(T), out List<Delegate> list))
                {
                    lock (list)
                    {
                        list.Remove(handler);
                    }
                }
            }
        }

        /// <summary>
        /// Veröffentlicht eine Nachricht an alle Abonnenten des angegebenen Typs.
        /// </summary>
        /// <typeparam name="T">Der Nachrichtentyp.</typeparam>
        /// <param name="message">Die zu veröffentlichende Nachricht.</param>
        public void Publish<T>(T message)
        {
            // Starke Abonnenten
            if (_subscribers.TryGetValue(typeof(T), out List<Delegate> list))
            {
                List<Delegate> copy;
                lock (list)
                {
                    copy = [.. list];
                }
                foreach (Delegate handler in copy)
                {
                    ((Action<T>)handler)?.Invoke(message);
                }
            }

            // Schwache Abonnenten
            if (_weakSubscribers.TryGetValue(typeof(T), out List<WeakReference> weakList))
            {
                List<WeakReference> toRemove = [];
                List<Action<T>> toInvoke = [];
                lock (weakList)
                {
                    foreach (WeakReference weakRef in weakList)
                    {
                        if (weakRef.Target is Action<T> action)
                        {
                            toInvoke.Add(action);
                        }
                        else if (!weakRef.IsAlive)
                        {
                            toRemove.Add(weakRef);
                        }
                    }
                    foreach (WeakReference dead in toRemove)
                    {
                        weakList.Remove(dead);
                    }
                }
                foreach (Action<T> action in toInvoke)
                {
                    action.Invoke(message);
                }
            }
        }
    }
}
