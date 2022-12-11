using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesmanProblem.Extensions
{
  public static class CollectionExtensions
  {
    public static bool IsInRange<T>(this ICollection<T> collection, int input)
      => input >= 0 && input < collection.Count;

    public static string CombineToString<T>(this IEnumerable<T> collection)
      => string.Join(", ", collection);

    public static void Swap<T>(this IList<T> list, int left, int right)
      => (list[left], list[right]) = (list[right], list[left]);

    public static int[][] DeepCopy(this int[][] input)
      => input.Select(m => m.ToArray()).ToArray();
  
    public static void ReverseSubList<T>(this IList<T> list, int left, int right)
    {
      while (left < right)
      {
        list.Swap(left++, right--);
      }
    }
    
    public static List<int> Shuffle(this IList<int> list)
    {
      var rand = new Random();
      return new List<int>(list.OrderBy(item => rand.Next()));
    }
  }  
}

