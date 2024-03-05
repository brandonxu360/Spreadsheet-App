// <copyright file="IExpTreeNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// The base class for all expression tree nodes.
/// </summary>
internal interface IExpTreeNode
{
    /// <summary>
    /// Evaluates the value of the node.
    /// </summary>
    /// <returns>The double value of the node.</returns>
    double Evaluate();
}