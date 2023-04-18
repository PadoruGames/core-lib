using System;
using System.Collections.Generic;

namespace Padoru.Core
{
    public class Notifier<T>
    {
        private readonly List<Action<T>> callbacks = new();

        public void Subscribe(Action<T> callback)
        {
            callbacks.Add(callback);
        }

        public void Unsubscribe(Action<T> callback)
        {
            callbacks.Remove(callback);
        }

        public void Notify(T value)
        {
            foreach (var callback in callbacks)
            {
                callback?.Invoke(value);
            }
        }
    }
}