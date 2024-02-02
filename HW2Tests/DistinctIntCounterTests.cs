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

    // COUNT WITH HASH SET TESTS METHODS

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

    /// <summary>
    /// Unit test for an unexpected list of chars.
    /// </summary>
    [Test]
    public void CountWithHashSet_CharList()
    {
        // Test case input - empty list
        List<int> list = ['a', 'b', 'a'];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithHashSet(list);

        // Result should be 2 distinct items ['a', 'b']
        // Function is still expected to operate normally
        Assert.That(result, Is.EqualTo(2));
    }

    /// <summary>
    /// Unit test for an unexpected null list.
    /// </summary>
    [Test]
    public void CountWithHashSet_NullList()
    {
        // Test case input - null
        List<int> list = null!;

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        // ArgumentNullException expected
        Assert.Throws<ArgumentNullException>(() => distinctIntCounter.CountWithHashSet(list));
    }

    // COUNT WITH O(1) SPACE METHODS

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

    /// <summary>
    /// Unit test for an unexpected list of chars.
    /// </summary>
    [Test]
    public void CountWithO1Space_CharList()
    {
        // Test case input - empty list
        List<int> list = ['a', 'b', 'a'];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithO1Space(list);

        // Result should be 2 distinct items ['a', 'b']
        // Function is still expected to operate normally
        Assert.That(result, Is.EqualTo(2));
    }

    /// <summary>
    /// Unit test for an unexpected null list.
    /// </summary>
    [Test]
    public void CountWithO1Space_NullList()
    {
        // Test case input - null
        List<int> list = null!;

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        // NullReferenceException expected
        Assert.Throws<NullReferenceException>(() => distinctIntCounter.CountWithO1Space(list));
    }

    // COUNT WITH SORT METHODS

    /// <summary>
    /// Unit test for an input list with five distinct integers and no duplicates.
    /// </summary>
    [Test]
    public void CountWithSort_AllDistinct()
    {
        // Test case input - five distinct integers
        List<int> list = [1, 2, 3, 4, 5];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithSort(list);

        Console.WriteLine(result);

        // Result should be 5 distinct integers [1, 2, 3, 4, 5]
        Assert.That(result, Is.EqualTo(5));
    }

    /// <summary>
    /// Unit test for a input list with 5 duplicate integers.
    /// </summary>
    [Test]
    public void CountWithSort_AllDuplicate()
    {
        // Test case input - input list with 5 duplicate integers
        List<int> list = [4, 4, 4, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithSort(list);

        // Result should be 1 distinct integer [4]
        Assert.That(result, Is.EqualTo(1));
    }

    /// <summary>
    /// Unit test for an input list of duplicates and distinct integers (mixed).
    /// </summary>
    [Test]
    public void CountWithSort_DistinctAndDuplicate()
    {
        // Test case input - empty list
        List<int> list = [1, 5, 3, 3, 5, 7, 6, 4, 4];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithSort(list);

        // Result should be 6 distinct integers [1, 3, 4, 5, 6, 7]
        Assert.That(result, Is.EqualTo(6));
    }

    /// <summary>
    /// Unit test for an empty input list.
    /// </summary>
    [Test]
    public void CountWithSort_EmptyList()
    {
        // Test case input - empty list
        List<int> list = [];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithSort(list);

        // Result should be 0 distinct integers []
        Assert.That(result, Is.EqualTo(0));
    }

    /// <summary>
    /// Unit test for an unexpected list of chars.
    /// </summary>
    [Test]
    public void CountWithSort_CharList()
    {
        // Test case input - empty list
        List<int> list = ['a', 'b', 'a'];

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        int result = distinctIntCounter.CountWithSort(list);

        // Result should be 2 distinct items ['a', 'b']
        // Function is still expected to operate normally
        Assert.That(result, Is.EqualTo(2));
    }

    /// <summary>
    /// Unit test for an unexpected null list.
    /// </summary>
    [Test]
    public void CountWithSort_NullList()
    {
        // Test case input - null
        List<int> list = null!;

        // Instantiate the distinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        // NullReferenceException expected
        Assert.Throws<NullReferenceException>(() => distinctIntCounter.CountWithSort(list));
    }
}