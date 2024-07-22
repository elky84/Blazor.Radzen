using System.Collections.Concurrent;

namespace MinimalApi.Common.Database;

public class ChangedBag<TDao>(Func<TDao, object> keySelector)
    where TDao : class
{
    private readonly ConcurrentDictionary<object, TDao> _items = new();
    private readonly Func<TDao, object> _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

    public IEnumerable<TDao> All => _items.Values;

    public void Upsert(TDao item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var key = _keySelector(item);
        if (key == null)
        {
            throw new ArgumentException("The key selector returned null for the provided item.");
        }

        Remove(key);
        _items[key] = item;
    }
    public TDao? Get(object key)
    {
        _items.TryGetValue(key, out var item);
        return item;
    }
    public bool Remove(object key)
    {
        return _items.Remove(key, out _);
    }
}