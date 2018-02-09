using System.Collections.Generic;

namespace Rallydator.Utils
{
    public static class ListExtensions
    {
        public static void AddDistinct<T>(this List<T> obj, T item)
        {
            if (obj.Contains(item)) return;
            obj.Add(item);
        }

        public static void AddNotNull<T>(this List<T> obj, T item)
        {
            if (item == null) return;
            obj.Add(item);
        }
    }
}
