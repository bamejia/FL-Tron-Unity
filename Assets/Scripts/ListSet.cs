using System.Collections.Generic;


namespace Utils
{
    public class ListSet<T> : List<T>
    {
        private readonly HashSet<T> set = new();
        public new void Add(T item)
        {
            if (set.Add(item))
                base.Add(item);
        }

        public void Delete(T item)
        {
            if (set.Remove(item))
                base.Remove(item);
        }
    }
}

