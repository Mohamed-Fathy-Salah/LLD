using System.Collections.Concurrent;

public class Repository
{
    private ConcurrentBag<Book> Books { get; } = new();
    private ConcurrentDictionary<Member, List<Activity>> Members { get; } = new();

    public void AddBook(Book book)
    {
        Books.Add(book);
    }

    public Activity BookBorrowed(Member member, Book book, DateTime from, DateTime to)
    {
        if (!Members.ContainsKey(member))
        {
            Members[member] = new List<Activity>();
        }
        var activity = new Activity(member, book, from, to);
        Members[member].Add(activity);
        return activity;
    }

    public void BookReturned(Member member, Book book, DateTime returnDate)
    {
        if (Members.TryGetValue(member, out var activities))
        {
            var activity = activities.FirstOrDefault(a => a.Book == book && a.ActualReturnDate == null);
            if (activity != null)
            {
                activity.ReturnBook();
            }
        }
    }

    public List<Book> GetBooksByMember(Member member)
    {
        if (Members.TryGetValue(member, out var activities))
        {
            return activities.Select(a => a.Book).ToList();
        }
        return new List<Book>();
    }

    public List<Activity> GetActivitiesByMember(Member member)
    {
        if (Members.TryGetValue(member, out var activities))
        {
            return activities;
        }
        return new List<Activity>();
    }

    public List<Book> GetAllBooks()
    {
        return Books.ToList();
    }
}
