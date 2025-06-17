public class LibrarySystem(Repository repository, Rules rules)
{
    public Activity? Borrow(Member member, Book book)
    {
        if (rules.CanBorrow(member, book) && book.TryBook(member))
        {
            member.BorrowBook();
            return repository.BookBorrowed(member, book, DateTime.UtcNow, rules.GetReturnDateFromNow());
        }
        return null;
    }

    public bool Return(Member member, Book book)
    {
        if (book.TryReturn(member))
        {
            member.ReturnBook();
            repository.BookReturned(member, book, DateTime.UtcNow);
            return true;
        }
        return false;
    }
}
