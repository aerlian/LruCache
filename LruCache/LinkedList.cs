using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LruCacheSpecs")]
namespace Main
{
    internal class LinkedList<TKey, TEntity>
    {
        internal Node<TKey, TEntity> dummyHead = new Node<TKey, TEntity>(default, default);
        internal Node<TKey, TEntity> head;
        internal Node<TKey, TEntity> dummyTail = new Node<TKey, TEntity>(default, default);
        internal Node<TKey, TEntity> tail = new Node<TKey, TEntity>(default, default);
        internal bool IsEmpty => head == dummyHead && tail == dummyTail;

        internal LinkedList()
        {
            head = dummyHead;
            tail = dummyTail;
            head.Next = tail;
            tail.Prev = head;
        }

        internal Node<TKey,TEntity> Add(TKey key, TEntity e)
        {
            var headNext = head.Next;
            var n = new Node<TKey, TEntity>(key, e) { Prev = dummyHead, Next = headNext };

            head = n;
            dummyHead.Next = head;
            headNext.Prev = n;          

            if (tail == dummyTail)
            {
                tail = n;
            }

            return n;
        }

        internal Node<TKey, TEntity> Remove(Node<TKey,TEntity> node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node == dummyHead || node == dummyTail)
            {
                return default;
            }

            if (IsEmpty || !node.IsValid)
            {
                throw new LruCacheException("Node is not available for removal");
            }

            var prev = node.Prev;
            var next = node.Next;

            prev.Next = next;
            next.Prev = prev;

            if (dummyHead.Next == dummyTail && dummyTail.Prev == dummyHead)
            {
                head = dummyHead;
                tail = dummyTail;

                return node;
            }

            head = dummyHead.Next;
            tail = dummyTail.Prev;

            node.Prev = null;
            node.Next = null;
            
            return node;
        }

        public IEnumerable<TEntity> Values
        {
            get
            {
                for(var n = dummyHead.Next; n != dummyTail; n = n.Next)
                {
                    yield return n.Value;
                }
            }
        }

    }
}
