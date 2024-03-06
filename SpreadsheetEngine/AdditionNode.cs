namespace SpreadsheetEngine;

using System.Diagnostics;

/// <summary>
/// Node class to represent and apply the addition operator.
/// </summary>
public class AdditionNode : OperatorNode
{
    /// <summary>
    /// Character symbol to identify/represent addition.
    /// </summary>
    public static char OperatorSymbol = '+';

    /// <summary>
    /// Returns the sum of the left and right children node values.
    /// </summary>
    /// <returns>The double sum of the left and right children node values.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented yet.</exception>
    public override double Evaluate()
    {
        Debug.Assert(this.LeftChild != null, nameof(this.LeftChild) + " != null");
        Debug.Assert(this.RightChild != null, nameof(this.RightChild) + " != null");

        var leftValue = this.LeftChild.Evaluate();
        var rightValue = this.RightChild.Evaluate();

        return leftValue + rightValue;
    }
}