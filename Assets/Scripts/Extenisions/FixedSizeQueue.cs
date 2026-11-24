using System.Collections.Generic;
using System.Linq;

public class FixedSizeQueue<T>
{
    private readonly Queue<T> _queue = new();
    private readonly int _maxSize;
    private readonly IEqualityComparer<T> _comparer;

    public FixedSizeQueue(int maxSize, IEqualityComparer<T> comparer = null)
    {
        _maxSize = maxSize;
        _comparer = comparer ?? EqualityComparer<T>.Default;
    }

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
        if (_queue.Count > _maxSize)
        {
            _queue.Dequeue();
        }
    }

    public float GetSimilarCount(T item)
    {
        var count = _queue.Where(x => _comparer.Equals(item, x));
        if (!count.Any()) return 1;

        return count.Count();
    }

    public T Dequeue() => _queue.Dequeue();
    public T Peek() => _queue.Peek();
    public int Count => _queue.Count;
}
