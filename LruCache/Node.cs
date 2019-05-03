using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LruCacheSpecs")]
namespace Main
{
    internal class Node<TK, TV>
    {
        internal bool IsValid => Prev != null && Next != null;
        internal Node<TK, TV> Prev { get; set; }
        internal TK Key { get; set; }
        internal TV Value { get; set; }
        internal Node<TK, TV> Next { get; set; }

        public Node(TK k, TV v)
        {
            Key = k;
            Value = v;            
        }

        public override string ToString()
        {
            return $"Node key:{Key}, value:{Value}";
        }
    }
}
