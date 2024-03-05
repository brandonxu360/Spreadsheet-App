// <copyright file="ExpressionTreeTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests;

using SpreadsheetEngine;

/// <summary>
/// Class to unit test the ExpressionTree functionality.
/// </summary>
[TestFixture]
internal class ExpressionTreeTests
{
    /// <summary>
    /// Tests the evaluate method of ValueNode in a normal case.
    /// </summary>
    [Test]
    public void ValueNodeEvaluateTest()
    {
        // Arrange
        const double value = 10.5;
        var valueNode = new ValueNode(value);

        // Act
        var result = valueNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    /// <summary>
    /// Tests the evaluate method of VariableNode in a normal case.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateTest()
    {
        // Arrange
        const string name = "x";
        const double expectedValue = 20.7;
        var varDict = new Dictionary<string, double>
        {
            { "x", expectedValue },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(expectedValue));
    }

    /// <summary>
    /// Tests that the evaluate method of VariableNode returns the default value of 0 when a value is not found
    /// in the dictionary.
    ///
    /// * If variables are not set by the user, they can be default to 0 for this HW. In later HWs, once we
    ///   learn how to deal with exceptions, we will change this.
    /// </summary>
    [Test]
    public void VariableNodeEvaluateReturnsDefaultWhenVariableNotInDictionary()
    {
        // Arrange
        const string name = "y"; // Variable name not present in dictionary
        var varDict = new Dictionary<string, double>
        {
            { "x", 20.7 },
        };
        var variableNode = new VariableNode(varDict, name);

        // Act
        var result = variableNode.Evaluate();

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
}