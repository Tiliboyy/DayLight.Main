using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DayLight.Dependency.Lists;

public class ObservedList<T> : ICollection<T>, INotifyPropertyChanged
{

    public readonly List<T> List = new List<T>();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => List.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => List.GetEnumerator();
    
    public void Add(T item)
    {
        List.Add(item);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Add)));
    }
    public void Clear()
    {
        List.Clear();
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clear)));
    }
    public bool Contains(T item) => List.Contains(item);
    public void CopyTo(T[] array, int arrayIndex)
    {
        List.CopyTo(array, arrayIndex);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clear)));
    }
    public bool Remove(T item)
    {
        var remove = List.Remove(item);
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Remove)));
        return remove;
    }

    public int Count => List.Count;

    public bool IsReadOnly { get; } = false;



    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
