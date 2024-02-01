// <copyright file="DistinctIntCounterTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HW2Tests;

using HW2;

/// <summary>
/// Unit tests for DistinctIntCounter service.
/// </summary>
public class DistinctIntCounterTests
{
    /// <summary>
    /// Setup for the unit tests.
    /// </summary>
    [SetUp]
    public void Setup()
    {
    }

    // COUNT WITH HASH SET TESTS

    /// <summary>
    /// Unit test for an input list with five distinct integers and no duplicates.
    /// </summary>
    [Test]
    public void CountWithHashSet_AllDistinct()
    {
        // Test case input - five distinct integers
        List<int> list = [1, 2, 3, 4, 5];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithHashSet(list);

        // Result should be 5 distinct integers [1, 2, 3, 4, 5]
        Assert.That(result, Is.EqualTo(5));
    }

    /// <summary>
    /// Unit test for a input list with 5 duplicate integers.
    /// </summary>
    [Test]
    public void CountWithHashSet_AllDuplicate()
    {
        // Test case input - input list with 5 duplicate integers
        List<int> list = [4, 4, 4, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithHashSet(list);

        // Result should be 1 distinct integer [4]
        Assert.That(result, Is.EqualTo(1));
    }

    /// <summary>
    /// Unit test for an input list of duplicates and distinct integers (mixed).
    /// </summary>
    [Test]
    public void CountWithHashSet_DistinctAndDuplicate()
    {
        // Test case input - empty list
        List<int> list = [1, 5, 3, 3, 5, 7, 6, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithHashSet(list);

        // Result should be 6 distinct integers [1, 3, 4, 5, 6, 7]
        Assert.That(result, Is.EqualTo(6));
    }

    /// <summary>
    /// Unit test for an empty input list.
    /// </summary>
    [Test]
    public void CountWithHashSet_EmptyList()
    {
        // Test case input - empty list
        List<int> list = [];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithHashSet(list);

        // Result should be 0 distinct integers []
        Assert.That(result, Is.EqualTo(0));
    }

    // COUNT WITH O(1) SPACE

    /// <summary>
    /// Unit test for an input list with five distinct integers and no duplicates.
    /// </summary>
    [Test]
    public void CountWithO1Space_AllDistinct()
    {
        // Test case input - five distinct integers
        List<int> list = [1, 2, 3, 4, 5];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithO1Space(list);

        Console.WriteLine(result);

        // Result should be 5 distinct integers [1, 2, 3, 4, 5]
        Assert.That(result, Is.EqualTo(5));
    }

    /// <summary>
    /// Unit test for a input list with 5 duplicate integers.
    /// </summary>
    [Test]
    public void CountWithO1Space_AllDuplicate()
    {
        // Test case input - input list with 5 duplicate integers
        List<int> list = [4, 4, 4, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithO1Space(list);

        // Result should be 1 distinct integer [4]
        Assert.That(result, Is.EqualTo(1));
    }

    /// <summary>
    /// Unit test for an input list of duplicates and distinct integers (mixed).
    /// </summary>
    [Test]
    public void CountWithO1Space_DistinctAndDuplicate()
    {
        // Test case input - empty list
        List<int> list = [1, 5, 3, 3, 5, 7, 6, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithO1Space(list);

        // Result should be 6 distinct integers [1, 3, 4, 5, 6, 7]
        Assert.That(result, Is.EqualTo(6));
    }

    /// <summary>
    /// Unit test for an empty input list.
    /// </summary>
    [Test]
    public void CountWithO1Space_EmptyList()
    {
        // Test case input - empty list
        List<int> list = [];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithO1Space(list);

        // Result should be 0 distinct integers []
        Assert.That(result, Is.EqualTo(0));
    }
}