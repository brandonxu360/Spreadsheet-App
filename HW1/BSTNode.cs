using System.Net.Sockets;

namespace HW1
{
    public class BSTNode
    {
        // Right and Left child nodes with initial value of null
        public BSTNode RNode { get; private set; } = null;
        public BSTNode LNode { get; private set; } = null;
        
        // Int value that the BSTNode represents 
        public int Value { get; private set; }
        
        // Constructor that takes a value and sets the Value property to the input value
        public BSTNode(int value)
        {
            Value = value;
        }
        
        // Recursive add function
        // Returns true if new node is added, false if not (duplicate found)
        // Duplicate values will not be added
        public bool Add(int value)
        {
            if (value < Value) // Look left case
            {
                // If left child exists, continue traversing left
                if (LNode != null) return LNode.Add(value);
                
                // If left child is null, set new node as left child
                LNode = new BSTNode(value);
                return true;
            }

            else if (value > Value) // Look right case
            {
                // If right child exists, continue traversing right
                if (RNode != null) return RNode.Add(value);
                
                // If right child is null, set new node as right child
                RNode = new BSTNode(value);
                return true;
            }
            
            // Return false in case of duplicate
            return false;
        }

    }
}