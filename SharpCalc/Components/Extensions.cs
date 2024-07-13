using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCalc.Components
{
    internal static class Extensions
    {
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> left, IEnumerable<T> right, IEqualityComparer<T> comparer)
        {
            var myList = right.ToList();
            foreach (var item in left)
            {
                var index = myList.FindIndex(n => comparer.Equals(item, n));
                if (index >= 0) myList.RemoveAt(index);
                else yield return item;
            }
            yield break;
        }
        public static IEnumerable<T> Exclude<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            return left.Exclude(right, EqualityComparer<T>.Default);
        }
        public static (List<T> AMinB, List<T> Intersection, List<T> BMinA) Intersection<T>(this IEnumerable<T> _left, IEnumerable<T> _right, IEqualityComparer<T> comparer)
        {
            var left = _left.ToList();
            var right = _right.ToList();
            var intersection = new List<T>();
            for (int i = 0; i < left.Count; i++)
            {
                var j = right.FindIndex(n => comparer.Equals(n, left[i]));
                if (j != -1)
                {
                    intersection.Add(left[i]);
                    left.RemoveAt(i);
                    right.RemoveAt(j);
                    i--;
                }
            }
            return (left, intersection, right);
        }
        public static (List<T> AMinB, List<T> Intersection, List<T> BMinA) Intersection<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            return left.Intersection(right, EqualityComparer<T>.Default);
        }
    }
}
