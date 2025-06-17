public class Book(string title, string author, string isbn, short publicationYear)
{
    public string Title { get; } = title;
    public string Author { get; } = author;
    public string ISBN { get; } = isbn;
    public short PublicationYear { get; } = publicationYear;
    public Member? Borrower { get; private set; }

    public bool IsAvailable()
    {
        lock (this)
        {
            return Borrower == null;
        }
    }

    public bool TryBook(Member member)
    {
        lock (this)
        {
            if (Borrower == null)
            {
                Borrower = member;
                return true;
            }
            return false;
        }
    }
    public bool TryReturn(Member member)
    {
        lock (this)
        {
            if (Borrower != null && Borrower == member)
            {
                Borrower = null;
                return true;
            }
            return false;
        }
    }
}
