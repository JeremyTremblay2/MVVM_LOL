using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utils
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
        }

        public new bool Remove(TKey key)
        {
            if (base.ContainsKey(key))
            {
                TValue value = base[key];
                bool result = base.Remove(key);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));
                return result;
            }
            return false;
        }

        public new TValue this[TKey key]
        {
            get { return base[key]; }
            set
            {
                base[key] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, value));
            }
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}

