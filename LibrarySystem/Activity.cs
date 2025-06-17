public class Activity(Member member, Book book, DateTime borrowDate, DateTime returnDate)
{
    public Member Member { get; set; } = member;
    public Book Book { get; set; } = book;
    public DateTime BorrowDate { get; } = borrowDate;
    public DateTime ReturnDate { get; } = returnDate;
    public DateTime ActualReturnDate { get; private set; }

    public void ReturnBook()
    {
        if (ActualReturnDate != null)
        {
            Console.WriteLine("Book has already been returned.");
            return;
        }
        ActualReturnDate = DateTime.UtcNow;
    }
}
