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
    /// <param name="list">The list of integers to count the number of distinct integers from.</param>
    /// <returns>The number of distinct integers in the input list.</returns>
    public int CountWithHashSet(List<int> list)
    {
        HashSet<int> hashSet = [..list];
        return hashSet.Count;
    }

    /// <summary>
    /// Counts the number of distinct integers in the input list using O(1) space - no dynamic memory allocation
    /// </summary>
    /// <param name="list">The list of integers to count the number of distinct integers from.</param>
    /// <returns>The number of distinct integers in the input list.</returns>
    public int CountWithO1Space(List<int> list)
    {
        int distinctIntCount = 0;
        int listLength = list.Count;

        // For each item in the list, iterate through the rest of the list to check for duplicates
        for (int i = 0; i < listLength; i++)
        {
            // Flag to indicate if the current list item is distinct
            bool isDistinct = true;

            // Iterate through the rest of the list starting at the item immediately after the current item
            for (int j = i + 1; j < listLength; j++)
            {
                // If a duplicate is found for an element in the list set the distinct flag to false
                if (list[i] == list[j])
                {
                    isDistinct = false;
                    break; // There is no need to check for further duplicates after one is found
                }
            }

            // Increment the distinct counter if no duplicates were found
            if (isDistinct)
            {
                distinctIntCount++;
            }
        }

        return distinctIntCount;
    }
}