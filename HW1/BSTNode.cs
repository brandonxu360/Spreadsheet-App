namespace HW1
{
    public class BSTNode
    {
        // Right and Left child nodes with initial value of null
        public BSTNode RNode { get; private set; } = null;
        public BSTNode LNode { get; private set; } = null;
        
        // Int value that the BSTNode represents 
        public int Value { get; private set; }
    }
}