namespace BooleanMinimizerLibrary
{
    public enum NodeType
    {
        Variable,
        Constant,
        Vector,
        Not,
        And,
        Or,
        Xor,
        Implies,
        Equivalent
    }

    public class Node
    {
        public NodeType Type { get; set; }
        public string Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public List<string> Variables { get; set; } // Новое свойство

        public Node(NodeType type, string value = null, Node left = null, Node right = null)
        {
            Type = type;
            Value = value;
            Left = left;
            Right = right;
        }
    }
}
