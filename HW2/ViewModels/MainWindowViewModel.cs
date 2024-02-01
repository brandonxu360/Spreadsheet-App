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

    private string RunDistinctIntegers()
    {
        // Instantiate an instance of the DistinctIntCounter service
        DistinctIntCounter distinctIntCounter = new DistinctIntCounter();

        // Create the random list of 10,000 random integers in the range [0, 20,000]
        List<int> randomIntList = this.GenerateRandomIntList(10000, 0, 20000);

        return "Default Return";
    }

    private List<int> GenerateRandomIntList(int count, int minValue, int maxValue)
    {
        var randomList = new List<int>();
        var random = new Random();

        for (var i = 0; i < count; i++)
        {
            randomList.Add(random.Next(minValue, maxValue + 1));
        }

        return randomList;
    }
}
