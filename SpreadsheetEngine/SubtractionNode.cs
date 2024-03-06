namespace SpreadsheetEngine;

/// <summary>
/// Node class to represent and apply the subtraction operator.
/// </summary>
public class SubtractionNode : OperatorNode
{
    /// <summary>
    /// Character symbol to identify/represent subtraction.
    /// </summary>
    public static char OperatorSymbol = '-';

    /// <summary>
    /// Returns the difference of the left and right children node values.
    /// </summary>
    /// <returns>The double difference of the left and right children node values.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented yet.</exception>
    public override double Evaluate()
    {
        throw new NotImplementedException();
    }
}