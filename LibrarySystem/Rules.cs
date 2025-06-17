public class Rules(Repository repository, int maxNumberOfBooks, int maxBorrowDurationDays)
{
    public Repository Repository { get; } = repository;
    public int MaxNumberOfBooks { get; } = maxNumberOfBooks;
    public int MaxBorrowDurationDays { get; } = maxBorrowDurationDays;

    public bool CanBorrow(Member member, Book book) {
        return book.IsAvailable() && member.BorrowedBooksCount < MaxNumberOfBooks;
    }

    public DateTime GetReturnDateFromNow() {
        return DateTime.UtcNow.AddDays(MaxBorrowDurationDays);
    }
}
