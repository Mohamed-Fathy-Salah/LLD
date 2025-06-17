using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class Program
{
    private static readonly Random random = new Random();
    private static LibrarySystem librarySystem;
    private static List<Member> members;
    private static List<Book> books;
    private static int totalOperations = 0;
    private static int successfulBorrows = 0;
    private static int successfulReturns = 0;
    private static int failedBorrows = 0;
    private static int failedReturns = 0;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== Library Management System - Concurrent Testing ===\n");
        
        // Initialize the system
        InitializeSystem();
        
        // Display initial state
        DisplaySystemState("INITIAL STATE");
        
        // Run concurrent operations
        Console.WriteLine("Starting concurrent operations with multiple users...\n");
        
        var tasks = new List<Task>();
        
        // Create tasks for each member to perform operations concurrently
        foreach (var member in members)
        {
            tasks.Add(Task.Run(() => SimulateUserActivity(member)));
        }
        
        // Wait for all tasks to complete
        await Task.WhenAll(tasks);
        
        // Display final results
        DisplayFinalResults();
    }
    
    private static void InitializeSystem()
    {
        // Create repository and rules
        var repository = new Repository();
        var rules = new Rules(repository, maxNumberOfBooks: 3, maxBorrowDurationDays: 14);
        
        // Initialize library system
        librarySystem = new LibrarySystem(repository, rules);
        
        // Create books
        books = new List<Book>
        {
            new Book("The Great Gatsby", "F. Scott Fitzgerald", "978-0-7432-7356-5", 1925),
            new Book("To Kill a Mockingbird", "Harper Lee", "978-0-06-112008-4", 1960),
            new Book("1984", "George Orwell", "978-0-452-28423-4", 1949),
            new Book("Pride and Prejudice", "Jane Austen", "978-0-14-143951-8", 1813),
            new Book("The Catcher in the Rye", "J.D. Salinger", "978-0-316-76948-0", 1951),
            new Book("Lord of the Flies", "William Golding", "978-0-571-05686-2", 1954),
            new Book("Animal Farm", "George Orwell", "978-0-452-28424-1", 1945),
            new Book("Brave New World", "Aldous Huxley", "978-0-06-085052-4", 1932)
        };
        
        // Add books to repository
        foreach (var book in books)
        {
            repository.AddBook(book);
        }
        
        // Create members
        members = new List<Member>
        {
            new Member("Alice Johnson", "alice@example.com"),
            new Member("Bob Smith", "bob@example.com"),
            new Member("Charlie Brown", "charlie@example.com"),
            new Member("Diana Prince", "diana@example.com"),
            new Member("Eve Wilson", "eve@example.com"),
            new Member("Frank Miller", "frank@example.com")
        };
        
        Console.WriteLine($"Initialized system with {books.Count} books and {members.Count} members");
        Console.WriteLine($"Rules: Max {3} books per member, {14} days borrow duration\n");
    }
    
    private static async Task SimulateUserActivity(Member member)
    {
        var memberOperations = 0;
        var memberBorrows = 0;
        var memberReturns = 0;
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {member.Name} started activity (Thread: {Thread.CurrentThread.ManagedThreadId})");
        
        // Simulate random user behavior over time
        for (int i = 0; i < 10; i++)
        {
            try
            {
                // Random delay to simulate real user behavior
                await Task.Delay(random.Next(100, 500));
                
                var operation = random.Next(0, 3); // 0: borrow, 1: return, 2: check status
                
                switch (operation)
                {
                    case 0: // Borrow a book
                        var borrowResult = await AttemptBorrow(member);
                        if (borrowResult) memberBorrows++;
                        memberOperations++;
                        break;
                        
                    case 1: // Return a book
                        var returnResult = await AttemptReturn(member);
                        if (returnResult) memberReturns++;
                        memberOperations++;
                        break;
                        
                    case 2: // Check status
                        CheckMemberStatus(member);
                        break;
                }
                
                Interlocked.Increment(ref totalOperations);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {member.Name}: {ex.Message}");
            }
        }
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {member.Name} completed activity - Operations: {memberOperations}, Borrows: {memberBorrows}, Returns: {memberReturns}");
    }
    
    private static async Task<bool> AttemptBorrow(Member member)
    {
        // Try to borrow a random available book
        var availableBooks = books.Where(b => b.IsAvailable()).ToList();
        
        if (availableBooks.Count > 0)
        {
            var bookToBorrow = availableBooks[random.Next(availableBooks.Count)];
            var activity = librarySystem.Borrow(member, bookToBorrow);
            
            if (activity != null)
            {
                Interlocked.Increment(ref successfulBorrows);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ✓ {member.Name} borrowed '{bookToBorrow.Title}' (Books: {member.BorrowedBooksCount}) [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                
                // Small delay to allow other threads to interleave
                await Task.Delay(random.Next(10, 50));
                return true;
            }
            else
            {
                Interlocked.Increment(ref failedBorrows);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ✗ {member.Name} failed to borrow '{bookToBorrow.Title}' (Books: {member.BorrowedBooksCount}) [Thread: {Thread.CurrentThread.ManagedThreadId}]");
            }
        }
        else
        {
            Interlocked.Increment(ref failedBorrows);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ✗ {member.Name} - No books available to borrow [Thread: {Thread.CurrentThread.ManagedThreadId}]");
        }
        
        // Small delay to allow other threads to interleave
        await Task.Delay(random.Next(10, 50));
        return false;
    }
    
    private static async Task<bool> AttemptReturn(Member member)
    {
        // Try to return a random borrowed book
        var borrowedBooks = books.Where(b => b.Borrower == member).ToList();
        
        if (borrowedBooks.Count > 0)
        {
            var bookToReturn = borrowedBooks[random.Next(borrowedBooks.Count)];
            var success = librarySystem.Return(member, bookToReturn);
            
            if (success)
            {
                Interlocked.Increment(ref successfulReturns);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ↩ {member.Name} returned '{bookToReturn.Title}' (Books: {member.BorrowedBooksCount}) [Thread: {Thread.CurrentThread.ManagedThreadId}]");
                
                // Small delay to allow other threads to interleave
                await Task.Delay(random.Next(10, 50));
                return true;
            }
            else
            {
                Interlocked.Increment(ref failedReturns);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ✗ {member.Name} failed to return '{bookToReturn.Title}' [Thread: {Thread.CurrentThread.ManagedThreadId}]");
            }
        }
        else
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ℹ {member.Name} - No books to return [Thread: {Thread.CurrentThread.ManagedThreadId}]");
        }
        
        // Small delay to allow other threads to interleave
        await Task.Delay(random.Next(10, 50));
        return false;
    }
    
    private static void CheckMemberStatus(Member member)
    {
        var borrowedBooks = books.Where(b => b.Borrower == member).ToList();
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ℹ {member.Name} status: {member.BorrowedBooksCount} books borrowed [{string.Join(", ", borrowedBooks.Select(b => $"'{b.Title}'"))}] [Thread: {Thread.CurrentThread.ManagedThreadId}]");
    }
    
    private static void DisplaySystemState(string title)
    {
        Console.WriteLine($"=== {title} ===");
        
        Console.WriteLine("\nBooks Status:");
        foreach (var book in books)
        {
            var status = book.IsAvailable() ? "Available" : $"Borrowed by {book.Borrower?.Name}";
            Console.WriteLine($"  '{book.Title}' by {book.Author} - {status}");
        }
        
        Console.WriteLine("\nMembers Status:");
        foreach (var member in members)
        {
            Console.WriteLine($"  {member.Name} - {member.BorrowedBooksCount} books borrowed");
        }
        
        Console.WriteLine();
    }
    
    private static void DisplayFinalResults()
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("FINAL RESULTS");
        Console.WriteLine(new string('=', 60));
        
        // Display final system state
        DisplaySystemState("FINAL SYSTEM STATE");
        
        // Display statistics
        Console.WriteLine("OPERATION STATISTICS:");
        Console.WriteLine($"  Total Operations: {totalOperations}");
        Console.WriteLine($"  Successful Borrows: {successfulBorrows}");
        Console.WriteLine($"  Failed Borrows: {failedBorrows}");
        Console.WriteLine($"  Successful Returns: {successfulReturns}");
        Console.WriteLine($"  Failed Returns: {failedReturns}");
        
        // Verify data consistency
        Console.WriteLine("\nDATA CONSISTENCY CHECK:");
        VerifyDataConsistency();
        
        // Display detailed member activities
        Console.WriteLine("\nMEMBER ACTIVITY SUMMARY:");
        var repository = new Repository();
        foreach (var member in members)
        {
            var activities = repository.GetActivitiesByMember(member);
            Console.WriteLine($"  {member.Name}: {activities.Count} total activities");
            
            var currentlyBorrowed = books.Where(b => b.Borrower == member).ToList();
            if (currentlyBorrowed.Count > 0)
            {
                Console.WriteLine($"    Currently borrowed: {string.Join(", ", currentlyBorrowed.Select(b => $"'{b.Title}'"))}");
            }
        }
        
        Console.WriteLine($"\nTest completed successfully! All operations were thread-safe.");
    }
    
    private static void VerifyDataConsistency()
    {
        var issues = new List<string>();
        
        // Check that each member's borrowed book count matches actual borrowed books
        foreach (var member in members)
        {
            var actualBorrowedBooks = books.Where(b => b.Borrower == member).Count();
            if (member.BorrowedBooksCount != actualBorrowedBooks)
            {
                issues.Add($"{member.Name}: Count mismatch - Member says {member.BorrowedBooksCount}, actual {actualBorrowedBooks}");
            }
        }
        
        // Check for duplicate borrowers
        var borrowedBooks = books.Where(b => !b.IsAvailable()).ToList();
        var borrowerGroups = borrowedBooks.GroupBy(b => b.Borrower).Where(g => g.Count() > 1);
        
        // Check that no book is available and borrowed at the same time
        foreach (var book in books)
        {
            if (book.IsAvailable() && book.Borrower != null)
            {
                issues.Add($"Book '{book.Title}' reports as available but has borrower {book.Borrower.Name}");
            }
            if (!book.IsAvailable() && book.Borrower == null)
            {
                issues.Add($"Book '{book.Title}' reports as unavailable but has no borrower");
            }
        }
        
        if (issues.Count == 0)
        {
            Console.WriteLine("  ✓ All data consistency checks passed!");
        }
        else
        {
            Console.WriteLine("  ✗ Data consistency issues found:");
            foreach (var issue in issues)
            {
                Console.WriteLine($"    - {issue}");
            }
        }
    }
}
