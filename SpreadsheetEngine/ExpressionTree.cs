// <copyright file="ExpressionTree.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// The class responsible for creating, manipulating, and evaluating expression trees.
/// </summary>
public class ExpressionTree
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
    /// </summary>
    /// <param name="expression">The expression to construct the tree from.</param>
    public ExpressionTree(string expression)
    {
        // TODO: Implement constructor to construct tree from the specific expression
    }

    /// <summary>
    /// Sets the specified variable within the ExpressionTree variables dictionary.
    /// </summary>
    /// <param name="variableName">The name of the variable (key).</param>
    /// <param name="variableValue">The value of the variable (value).</param>
    public void SetVariable(string variableName, double variableValue)
    {
        // TODO: Implement method to set the specified variable within the ExpressionTree variables dictionary
    }

    /// <summary>
    /// Evaluates the expression to a double value.
    /// </summary>
    /// <returns>Double value that the expression evaluates to.</returns>
    /// <exception cref="NotImplementedException">Placeholder exception while the method is not implemented.</exception>
    public double Evaluate()
    {
        // TODO: Implement method with no parameters that evaluates the expression to double value
        throw new NotImplementedException();
    }
}