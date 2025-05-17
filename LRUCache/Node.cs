public class Node<T>(T value)
{
    public T Value { get; set; } = value;
    public Node<T>? next { get; set; } = null;
    public Node<T>? prev { get; set; } = null;
}
