namespace SpreadsheetEngine;

/// <summary>
/// Node class to represent and apply the division operator.
/// </summary>
public class DivisionNode : OperatorNode
{
    /// <summary>
    /// Character symbol to identify/represent division.
    /// </summary>
    public static char OperatorSymbol = '/';

    /// <summary>
    /// Returns the result of the left child node value divided by the right child node value.
    /// </summary>
    /// <returns>The double result of the left child node value divided by the left child node value.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented yet.</exception>
    public override double Evaluate()
    {
        throw new NotImplementedException();
    }
}