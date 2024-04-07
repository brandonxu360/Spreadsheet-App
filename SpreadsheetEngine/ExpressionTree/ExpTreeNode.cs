// <copyright file="ExpTreeNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine.ExpressionTree;

/// <summary>
/// The base class for all expression tree nodes.
/// </summary>
public abstract class ExpTreeNode
{
    /// <summary>
    /// Evaluates the value of the node.
    /// </summary>
    /// <returns>The double value of the node.</returns>
    public abstract double Evaluate();
}