// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW2.ViewModels;

using System;
using System.Collections.Generic;

/// <summary>
/// MainWindowViewModel.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
    /// </summary>
    public MainWindowViewModel()
    {
        this.Greeting = this.RunDistinctIntegers();
    }

    /// <summary>
    /// Gets or sets the Greeting property.
    /// </summary>
    public string Greeting { get; set; }

    /// <summary>
    /// Creates a list of 10,000 random integers in the range [0, 20000] and counts the number of
    /// distinct integers using a hash set method, a O(1) time complexity method, and a sorted
    /// list method.
    /// </summary>
    /// <returns>String that complies the text results from each method.</returns>
    private string RunDistinctIntegers()
    {
        // Instantiate an instance of the DistinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        // Create the random list of 10,000 random integers in the range [0, 20,000]
        List<int> randomIntList = this.GenerateRandomIntList(10000, 0, 20000);

        // Get results for each method of counting distinct integers
        int hashSetCountResult = distinctIntCounter.CountWithHashSet(randomIntList);
        int o1SpaceCountResult = distinctIntCounter.CountWithO1Space(randomIntList);
        int sortCountResult = distinctIntCounter.CountWithSort(randomIntList);

        // Compile and return the results as a string
        string resultString = $"1. HashSet method: {hashSetCountResult} distinct integers\n";
        resultString += $"2. O(1) Space method: {o1SpaceCountResult} distinct integers\n";
        resultString += $"3. Sorted list method: {sortCountResult} distinct integers\n";

        return resultString;
    }

    /// <summary>
    /// Generates a list of random integers in the input range (inclusive) of the input size.
    /// </summary>
    /// <param name="count">The size of the generated list (number of random integers).</param>
    /// <param name="minValue">The minimum value for the randomly generated integers (inclusive).</param>
    /// <param name="maxValue">The maximum value for the randomly generated integers (inclusive).</param>
    /// <returns>The list of random integers.</returns>
    private List<int> GenerateRandomIntList(int count, int minValue, int maxValue)
    {
        // Initialize the new list
        var randomList = new List<int>();

        // Instantiate the random object
        var random = new Random();

        // Generate the input amount of random integers
        for (var i = 0; i < count; i++)
        {
            // Add a random integer from the input range (inclusive)
            randomList.Add(random.Next(minValue, maxValue + 1));
        }

        return randomList;
    }
}