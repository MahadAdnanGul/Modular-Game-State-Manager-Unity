using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// Helper class for collections
/// </summary>
public static class CollectionHelper
{
    /// <summary>
    /// Shuffle collection
    /// </summary>
    /// <param name="list">List to shuffle</param>
    /// <typeparam name="T">Type of the object in the list</typeparam>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Shortcut to detect empty collections
    /// </summary>
    /// <param name="enumerable">Collection</param>
    /// <typeparam name="T">Type of object in collection</typeparam>
    /// <returns>True if empty, false otherwise</returns>
    public static bool IsEmpty<T>(this ICollection<T> enumerable)
    {
        return enumerable.Count == 0;
    }
    
    /// <summary>
    /// Add an item in the correct sorting position
    /// </summary>
    /// <param name="this">Collection</param>
    /// <param name="item">Item to insert</param>
    /// <typeparam name="T">Type of the collection</typeparam>
    public static void AddSorted<T>(this List<T> @this, T item) where T: IComparable<T>
    {
        if (@this.Count == 0)
        {
            @this.Add(item);
            return;
        }
        
        if (@this[@this.Count - 1].CompareTo(item) <= 0)
        {
            @this.Add(item);
            return;
        }
        
        if (@this[0].CompareTo(item) >= 0)
        {
            @this.Insert(0, item);
            return;
        }
        
        int index = @this.BinarySearch(item);
        if (index < 0)
        {
            index = ~index;
        }

        @this.Insert(index, item);
    }
}
