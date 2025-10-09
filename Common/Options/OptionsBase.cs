using System.Diagnostics;
using KY.Core;

namespace KY.Generator;

[DebuggerDisplay("{Target}")]
public abstract class OptionsBase<TSelf>
    where TSelf : OptionsBase<TSelf>
{
    // ReSharper disable once StaticMemberInGenericType
    private static int index;
    protected TSelf? Global { get; }
    protected List<TSelf> Parents { get; } = [];
    protected string Target { get; }

    protected OptionsBase(TSelf? parent, TSelf? global, object? target = null)
    {
        this.Global = global;
        if (parent != null)
        {
            this.Parents.Add(parent);
        }
        this.Target = target == null ? index++.ToString() : target + " " + index++;
    }

    public void AddParent(TSelf parent)
    {
        parent.AssertIsNotNull(nameof(parent));
        this.Parents.AddIfNotExists(parent);
    }

    protected T GetOwnValue<T>(Func<TSelf, T?> getter, T defaultValue = default) where T : struct
    {
        return getter((TSelf)this) ?? (this.Global == null ? null : getter(this.Global)) ?? defaultValue;
    }

    protected T? GetOwnValue<T>(Func<TSelf, T?> getter, T? defaultValue = null) where T : class
    {
        return getter((TSelf)this) ?? (this.Global == null ? null : getter(this.Global)) ?? defaultValue;
    }

    protected T GetValue<T>(Func<TSelf, T?> getter, T defaultValue = default) where T : struct
    {
        Queue<TSelf> searchTargets = new();
        List<TSelf> searchedTargets = [];
        searchTargets.Enqueue((TSelf)this);
        while (searchTargets.Any())
        {
            TSelf current = searchTargets.Dequeue();
            searchedTargets.Add(current);
            T? result = getter(current) ?? (current.Global == null ? null : getter(current.Global));
            if (result != null)
            {
                return result.Value;
            }
            foreach (TSelf parent in current.Parents)
            {
                if (searchedTargets.Contains(parent))
                {
                    continue;
                }
                searchTargets.Enqueue(parent);
            }
            if (current.Global != null && !searchedTargets.Contains(current.Global))
            {
                searchTargets.Enqueue(current.Global);
            }
        }
        return defaultValue;
    }

    protected T? GetValue<T>(Func<TSelf, T?> getter, T? defaultValue = null) where T : class
    {
        Queue<TSelf> searchTargets = new();
        List<TSelf> searchedTargets = [];
        searchTargets.Enqueue((TSelf)this);
        while (searchTargets.Any())
        {
            TSelf current = searchTargets.Dequeue();
            searchedTargets.Add(current);
            T? result = getter(current) ?? (current.Global == null ? null : getter(current.Global));
            if (result != null)
            {
                return result;
            }
            foreach (TSelf parent in current.Parents)
            {
                if (searchedTargets.Contains(parent))
                {
                    continue;
                }
                searchTargets.Enqueue(parent);
            }
            if (current.Global != null && !searchedTargets.Contains(current.Global))
            {
                searchTargets.Enqueue(current.Global);
            }
        }
        return defaultValue;
    }

    protected IReadOnlyList<T> GetList<T>(Func<TSelf, IEnumerable<T>?> getter)
    {
        List<T> result = [];
        List<TSelf> searchedTargets = [];
        Queue<TSelf> searchTargets = new();
        searchTargets.Enqueue((TSelf)this);
        while (searchTargets.Any())
        {
            TSelf current = searchTargets.Dequeue();
            searchedTargets.Add(current);
            IEnumerable<T>? found = getter(current) ?? (current.Global == null ? null : getter(current.Global));
            found?.Where(x => !result.Contains(x)).ForEach(x => result.Add(x));
            foreach (TSelf parent in current.Parents)
            {
                if (searchedTargets.Contains(parent))
                {
                    continue;
                }
                searchTargets.Enqueue(parent);
            }
            if (current.Global != null && !searchedTargets.Contains(current.Global))
            {
                searchTargets.Enqueue(current.Global);
            }
        }
        return result;
    }

    protected IReadOnlyDictionary<TKey, TValue> GetDictionary<TKey, TValue>(Func<TSelf, Dictionary<TKey, TValue>?> getter)
    {
        Dictionary<TKey, TValue> result = new();
        List<TSelf> searchedTargets = [];
        Queue<TSelf> searchTargets = new();
        searchTargets.Enqueue((TSelf)this);
        while (searchTargets.Any())
        {
            TSelf current = searchTargets.Dequeue();
            searchedTargets.Add(current);
            Dictionary<TKey, TValue>? found = getter(current) ?? (current.Global == null ? null : getter(current.Global));
            found?.Where(x => !result.ContainsKey(x.Key)).ForEach(x => result.Add(x.Key, x.Value));
            foreach (TSelf parent in current.Parents)
            {
                if (searchedTargets.Contains(parent))
                {
                    continue;
                }
                searchTargets.Enqueue(parent);
            }
            if (current.Global != null && !searchedTargets.Contains(current.Global))
            {
                searchTargets.Enqueue(current.Global);
            }
        }
        return result;
    }
}
