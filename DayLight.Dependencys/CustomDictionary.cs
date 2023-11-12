using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DayLight.Dependencys;

public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();
    
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.GetEnumerator();
    
    
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Dictionary.Add(item.Key, item.Value);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Add)));
    }
    public void Clear()
    {
        Dictionary.Clear();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Add)));
    }
    public bool Contains(KeyValuePair<TKey, TValue> item) => Dictionary.ContainsKey(item.Key);
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var remove = Dictionary.Remove(item.Key);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Remove)));
        return remove;
    }

    public int Count => Dictionary.Count;

    public bool IsReadOnly { get; } = false;
    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);
    public void Add(TKey key, TValue value)
    {
        Dictionary.Add(key, value);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Add)));

    }
    public bool Remove(TKey key)
    {
        var remove = Dictionary.Remove(key);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Remove)));
        return remove;
    }
    public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

    public TValue this[TKey key]
    {
        get => Dictionary[key];
        set => Dictionary[key] = value;
    }

    public ICollection<TKey> Keys => Dictionary.Keys;
    public ICollection<TValue> Values => Dictionary.Values;


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
