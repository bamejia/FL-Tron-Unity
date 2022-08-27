using System.Collections.Generic;

namespace CustomCollection
{
    public class ListSet<T> : List<T>
    {
        
        private readonly HashSet<T> set = new();
        public new void Add(T item)
        {
            if (set.Add(item))
                base.Add(item);
        }

        public new bool Remove(T item)
        {
            if (set.Remove(item))
                return base.Remove(item);
            return false;
        }

        public new bool Contains(T item)
        {
            return set.Contains(item);
        }
    }
}

