using System.Collections.Generic;
using UnityEngine;


namespace Players
{
    public class ListSet<T> : List<T>
    {
        
        private readonly HashSet<T> set = new();
        public new void Add(T item)
        {
            if (set.Add(item))
                base.Add(item);
        }

        public new void Remove(T item)
        {
            if (set.Remove(item))
                base.Remove(item);
        }
    }
}

