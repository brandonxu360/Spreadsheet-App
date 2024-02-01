namespace HW2;

using System.Collections.Generic;

/// <summary>
/// DistinctIntCounter
/// This class provides methods to count the distinct integers in a given list using three different implementations:
///     - Using a hash set
///     - Without altering the list or dynamic memory allocation (O(1) storage)
///     - Sorting the list and without dynamic memory allocation (O(1) storage, O(n) time)
/// </summary>
public class DistinctIntCounter
{
    /// <summary>
    /// Counts the number of distinct integers in the input list using a hash set.
    /// </summary>
    /// <param name="list">The list of integers to count the number of distinct integers in.</param>
    /// <returns>The number of distinct integers in the input list.</returns>
    public int CountWithHashSet(List<int> list)
    {
        HashSet<int> hashSet = [..list];
        return hashSet.Count;
    }
}