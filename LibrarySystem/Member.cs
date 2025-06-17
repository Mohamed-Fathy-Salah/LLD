public class Member(string name, string email)
{
    private static int _nextId = 0;
    public int Id { get; } = Interlocked.Increment(ref _nextId);
    public string Name { get; set; } = name;
    public string Email { get; set; } = email;
    public int BorrowedBooksCount => _borrowedBooksCount;
    private int _borrowedBooksCount = 0;

    public void BorrowBook()
    {
        Interlocked.Increment(ref _borrowedBooksCount);
    }

    public void ReturnBook()
    {
        if (BorrowedBooksCount > 0)
        {
            Interlocked.Decrement(ref _borrowedBooksCount);
        }
    }

    public override string ToString()
    {
        return $"{Name}";
    }
}
