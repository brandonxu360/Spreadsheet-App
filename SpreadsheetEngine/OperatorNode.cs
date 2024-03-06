// <copyright file="OperatorNode.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine;

/// <summary>
/// Abstract class for all binary operator nodes.
/// </summary>
public abstract class OperatorNode : ExpTreeNode
{
    /// <summary>
    /// The reference to the left child node.
    /// </summary>
    public ExpTreeNode? LeftChild;

    /// <summary>
    /// The reference to the right child node.
    /// </summary>
    public ExpTreeNode? RightChild;

    /// <summary>
    /// Initializes a new instance of the <see cref="OperatorNode"/> class.
    /// </summary>
    protected OperatorNode()
    {
        this.LeftChild = null;
        this.RightChild = null;
    }
}