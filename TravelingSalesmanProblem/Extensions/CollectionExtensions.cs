﻿using System;
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
    
    public static void Shuffle(this List<int> list, Random rand)
    {
      var n = list.Count;
      while (n > 1) 
      {
        var k = rand.Next(n--);
        (list[n], list[k]) = (list[k], list[n]);
      }
    }
    
    public static void Fill<T>(this T[] originalArray, T with) 
    {
      for(var i = 0; i < originalArray.Length; i++)
      {
        originalArray[i] = with;
      }
    } 
  }  
}

