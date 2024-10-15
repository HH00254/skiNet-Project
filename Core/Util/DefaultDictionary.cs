using System;

namespace Core.Util;

public class DefaultDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;
    private readonly Func<TValue> _defaultValueFactory;

    public DefaultDictionary(Func<TValue> defaultValueFactory)
    {
        _dictionary = new Dictionary<TKey, TValue>();
        _defaultValueFactory = defaultValueFactory;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (_dictionary.TryGetValue(key, out TValue? value))
            {
                return value;
            }
            else
            {
                TValue defaultValue = _defaultValueFactory();
                _dictionary[key] = defaultValue;
                return defaultValue;
            }
        }
        set
        {
            _dictionary[key] = value;
        }
    }

    public void Add(TKey key, TValue value) => _dictionary.Add(key, value);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public bool Remove(TKey key) => _dictionary.Remove(key);

    public void Clear() => _dictionary.Clear();
}