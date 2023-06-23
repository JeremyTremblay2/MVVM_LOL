using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Utils
{
    public class ReadOnlyObservableDictionary<TKey, TValue> : ReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged
    {
        private readonly ObservableDictionary<TKey, TValue> _dictionary;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ReadOnlyObservableDictionary(ObservableDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            _dictionary = dictionary;
            _dictionary.CollectionChanged += Dictionary_CollectionChanged;
        }

        private void Dictionary_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}