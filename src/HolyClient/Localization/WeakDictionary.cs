using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace HolyClient.Localization;

internal class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    where TKey : class
    where TValue : class
{
    private readonly object locker = new();

    private bool bAlive = true;
    private ConditionalWeakTable<TKey, WeakKeyHolder> keyHolderMap = new();
    private Dictionary<WeakReference, TValue> valueMap = new(new ObjectReferenceEqualityComparer<WeakReference>());

    private Dictionary<TKey, TValue> CurrentDictionary
    {
        get
        {
            lock (locker)
            {
                ManualShrink();
                return valueMap.ToDictionary(p => (TKey)p.Key.Target, p => p.Value);
            }
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out var val))
                return val;

            throw new KeyNotFoundException();
        }

        set => Set(key, value, true);
    }

    public ICollection<TKey> Keys
    {
        get
        {
            lock (locker)
            {
                ManualShrink();
                return valueMap.Keys.Select(k => (TKey)k.Target).ToList();
            }
        }
    }

    public ICollection<TValue> Values
    {
        get
        {
            lock (locker)
            {
                ManualShrink();
                return valueMap.Select(p => p.Value).ToList();
            }
        }
    }

    public int Count
    {
        get
        {
            lock (locker)
            {
                ManualShrink();
                return valueMap.Count;
            }
        }
    }

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        if (!Set(key, value, false))
            throw new ArgumentException("Key already exists");
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        lock (locker)
        {
            keyHolderMap = new ConditionalWeakTable<TKey, WeakKeyHolder>();
            valueMap.Clear();
        }
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        object curVal = null;

        lock (locker)
        {
            if (!keyHolderMap.TryGetValue(item.Key, out var weakKeyHolder))
                return false;

            curVal = weakKeyHolder.WeakRef.Target;
        }

        return curVal?.Equals(item.Value) == true;
    }

    public bool ContainsKey(TKey key)
    {
        lock (locker)
        {
            return keyHolderMap.TryGetValue(key, out var weakKeyHolder);
        }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary<TKey, TValue>)CurrentDictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return CurrentDictionary.GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        lock (locker)
        {
            if (!keyHolderMap.TryGetValue(key, out var weakKeyHolder))
                return false;

            keyHolderMap.Remove(key);
            valueMap.Remove(weakKeyHolder.WeakRef);

            return true;
        }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        lock (locker)
        {
            if (!keyHolderMap.TryGetValue(item.Key, out var weakKeyHolder))
                return false;

            if (weakKeyHolder.WeakRef.Target?.Equals(item.Value) != true)
                return false;

            keyHolderMap.Remove(item.Key);
            valueMap.Remove(weakKeyHolder.WeakRef);

            return true;
        }
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        lock (locker)
        {
            if (!keyHolderMap.TryGetValue(key, out var weakKeyHolder))
            {
                value = default;
                return false;
            }

            value = valueMap[weakKeyHolder.WeakRef];
            return true;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }

    private void OnKeyDrop(WeakReference weakKeyRef)
    {
        lock (locker)
        {
            if (!bAlive)
                return;

            valueMap.Remove(weakKeyRef);
        }
    }

    public void ManualShrink()
    {
        foreach (var key in valueMap.Keys.Where(k => !k.IsAlive).ToList())
            valueMap.Remove(key);
    }

    private bool Set(TKey key, TValue val, bool isUpdateOkay)
    {
        lock (locker)
        {
            if (keyHolderMap.TryGetValue(key, out var weakKeyHolder))
            {
                if (!isUpdateOkay)
                    return false;

                valueMap[weakKeyHolder.WeakRef] = val;
                return true;
            }

            weakKeyHolder = new WeakKeyHolder(this, key);
            keyHolderMap.Add(key, weakKeyHolder);

            valueMap.Add(weakKeyHolder.WeakRef, val);

            return true;
        }
    }

    protected void Dispose(bool bManual)
    {
        if (bManual)
        {
            Monitor.Enter(locker);

            if (!bAlive)
                return;
        }

        try
        {
            keyHolderMap = null;
            valueMap = null;
            bAlive = false;
        }
        finally
        {
            if (bManual)
                Monitor.Exit(locker);
        }
    }

    ~WeakDictionary()
    {
        Dispose(false);
    }

    private class WeakKeyHolder
    {
        private readonly WeakDictionary<TKey, TValue> outer;

        public WeakKeyHolder(WeakDictionary<TKey, TValue> outer, TKey key)
        {
            this.outer = outer;
            WeakRef = new WeakReference(key);
        }

        public WeakReference WeakRef { get; }

        ~WeakKeyHolder()
        {
            outer?.OnKeyDrop(
                WeakRef); // Nullable operator used just in case this.outer gets set to null by GC before this finalizer runs. But I haven't had this happen.
        }
    }
}

internal class ObjectReferenceEqualityComparer<T> : IEqualityComparer<T>
{
    public static ObjectReferenceEqualityComparer<T> Default = new();

    public bool Equals(T x, T y)
    {
        return ReferenceEquals(x, y);
    }

    public int GetHashCode(T obj)
    {
        return RuntimeHelpers.GetHashCode(obj);
    }
}