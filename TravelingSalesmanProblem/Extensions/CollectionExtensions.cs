namespace TravelingSalesmanProblem.Extensions;

public static class CollectionExtensions
{
  public static bool IsInRange<T>(this ICollection<T> collection, int input)
    => input >= 0 && input < collection.Count;

  public static string CombineToString<T>(this IEnumerable<T> collection)
    => string.Join(", ", collection);

  public static void Swap<T>(this IList<T> list, int left, int right)
    => (list[left], list[right]) = (list[right], list[left]);

  public static void ReverseSubList<T>(this IList<T> list, int left, int right)
  {
    while (left < right)
    {
      list.Swap(left++, right--);
    }
  }
}