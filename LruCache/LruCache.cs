using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LruCacheSpecs")]
namespace Main
{
    public class LruCache<TKey, TValue>
    {
        private readonly int capacity;
        private readonly Dictionary<TKey, Node<TKey, TValue>> map = new Dictionary<TKey, Node<TKey, TValue>>();
        private readonly LinkedList<TKey, TValue> list = new LinkedList<TKey, TValue>();

        public bool IsEmpty => Count == 0;
        public int Count { get; private set; }

        public LruCache(int capacity)
        {
            if (capacity <= 0)
            {
                throw new LruCacheException($"{nameof(capacity)} must be greater than zero");
            }

            this.capacity = capacity;
        }

        internal Node<TKey,TValue> RemoveInternal(Node<TKey,TValue> node)
        {
            lock (list)
            {
                if (list.IsEmpty)
                {
                    throw new LruCacheException($"List is empty cannot remove node {node}");
                }

                list.Remove(node);
                map.Remove(node.Key);
                Count -= 1;

                return node;
            }            
        }

        public void AddOrUpdate(TKey key, TValue value)
        {
            lock (list)
            {
                if (map.TryGetValue(key, out Node<TKey, TValue> v))
                {
                    v.Value = value;
                    return;
                }

                if (Count >= capacity)
                {
                    RemoveInternal(list.tail); 
                }

                var node = list.Add(key, value);
                map.Add(key, node);
                Count += 1;
            }            
        }

        public void Remove(TKey key)
        {
            lock (list)
            {
                if (!map.TryGetValue(key, out Node<TKey, TValue> n))
                {
                    throw new LruCacheException($"Cache {nameof(key)} does not exist: {key}");
                }

                RemoveInternal(n);
                return;
            }            
        }

        public bool Find(TKey key, out TValue value)
        {
            lock (list)
            {
                value = default;

                if (!map.TryGetValue(key, out Node<TKey, TValue> node))
                {
                    return false;
                }

                value = node.Value;
                return true;
            }            
        }
    }
}
