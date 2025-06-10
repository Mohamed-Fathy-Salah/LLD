class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Online Auction System Demo ===\n");
        
        // Initialize services
        var authService = new AuthService();
        var searchService = new SearchService();
        var auctionSystem = new OnlineAuctionSystem(authService, searchService);
        
        // Create and register users
        var users = new[]
        {
            authService.SignUp("Alice Johnson", "alice@email.com", "password123"),
            authService.SignUp("Bob Smith", "bob@email.com", "password456"),
            authService.SignUp("Charlie Brown", "charlie@email.com", "password789"),
            authService.SignUp("Diana Prince", "diana@email.com", "passwordabc"),
            authService.SignUp("Eve Adams", "eve@email.com", "passworddef")
        };
        
        Console.WriteLine("Registered Users:");
        foreach (var user in users)
        {
            if (user != null)
                Console.WriteLine($"- {user.Name} ({user.Email})");
        }
        Console.WriteLine();
        
        // Create auctions with different time windows
        var now = DateTime.Now;
        var auctions = new[]
        {
            new Auction(users[0]!, "Gaming Laptop", "High-performance gaming laptop with RTX 4070", 
                       CategoryEnum.Electronics, 800m, now.AddSeconds(-10), now.AddMinutes(2)),
            
            new Auction(users[1]!, "Vintage Watch", "Rare 1960s Rolex in excellent condition", 
                       CategoryEnum.CollectiblesAndArt, 1500m, now.AddSeconds(5), now.AddMinutes(3)),
            
            new Auction(users[2]!, "Designer Jacket", "Limited edition leather jacket", 
                       CategoryEnum.Fashion, 200m, now.AddSeconds(-5), now.AddMinutes(1.5)),
            
            new Auction(users[3]!, "Tennis Racket", "Professional grade tennis racket", 
                       CategoryEnum.Sports, 150m, now.AddSeconds(10), now.AddMinutes(2.5)),
            
            new Auction(users[4]!, "Smartphone", "Latest flagship smartphone, unlocked", 
                       CategoryEnum.Electronics, 600m, now.AddSeconds(-15), now.AddMinutes(4))
        };
        
        // Add auctions to the system
        foreach (var auction in auctions)
        {
            auctionSystem.Auctions.Add(auction);
        }
        
        Console.WriteLine("Created Auctions:");
        foreach (var auction in auctions)
        {
            Console.WriteLine($"- {auction.Name}: ${auction.StartingPrice} ({auction.Category})");
            Console.WriteLine($"  Active: {auction.From:HH:mm:ss} - {auction.To:HH:mm:ss}");
            Console.WriteLine($"  Status: {(auction.IsActive() ? "ACTIVE" : "INACTIVE")}");
        }
        Console.WriteLine();
        
        // Demonstrate search functionality
        Console.WriteLine("=== Search Demo ===");
        var electronicsFilter = new Filter(category: CategoryEnum.Electronics);
        var electronicsAuctions = searchService.Search(auctionSystem.Auctions, electronicsFilter);
        Console.WriteLine($"Electronics auctions found: {electronicsAuctions.Length}");
        foreach (var auction in electronicsAuctions)
        {
            Console.WriteLine($"- {auction.Name}: ${auction.StartingPrice}");
        }
        
        var priceFilter = new Filter(minPrice: 500m, maxPrice: 1000m);
        var priceFilteredAuctions = searchService.Search(auctionSystem.Auctions, priceFilter);
        Console.WriteLine($"\nAuctions between $500-$1000: {priceFilteredAuctions.Length}");
        foreach (var auction in priceFilteredAuctions)
        {
            Console.WriteLine($"- {auction.Name}: ${auction.StartingPrice}");
        }
        Console.WriteLine();
        
        // Start bidding simulation with multiple threads
        Console.WriteLine("=== Starting Bidding Simulation ===");
        Console.WriteLine("Each user will run in their own thread and bid on random auctions\n");
        
        var cancellationTokenSource = new CancellationTokenSource();
        var biddingTasks = new List<Task>();
        
        // Create a thread for each user
        for (int i = 0; i < users.Length; i++)
        {
            var user = users[i];
            var userIndex = i;
            
            if (user != null)
            {
                var task = Task.Run(() => SimulateUserBidding(user, auctions, userIndex, cancellationTokenSource.Token));
                biddingTasks.Add(task);
            }
        }
        
        // Let the bidding run for a while
        await Task.Delay(TimeSpan.FromMinutes(5));
        
        // Cancel all bidding tasks
        cancellationTokenSource.Cancel();
        
        try
        {
            await Task.WhenAll(biddingTasks);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\n=== Bidding Simulation Completed ===");
        }
        
        // Display final results
        Console.WriteLine("\n=== Final Auction Results ===");
        foreach (var auction in auctions)
        {
            Console.WriteLine($"\nAuction: {auction.Name}");
            Console.WriteLine($"Category: {auction.Category}");
            Console.WriteLine($"Starting Price: ${auction.StartingPrice:F2}");
            Console.WriteLine($"Final Highest Bid: ${auction.HighestBid:F2}");
            Console.WriteLine($"Winner: {auction.HighestBidder?.Name ?? "No bids"}");
            Console.WriteLine($"Total Bidders: {auction.Bidders.Count}");
            Console.WriteLine($"Status: {(auction.IsActive() ? "Still Active" : "Ended")}");
        }
        
        // Demonstrate authentication
        Console.WriteLine("\n=== Authentication Demo ===");
        var signInResult = authService.SignIn("alice@email.com", "password123");
        Console.WriteLine($"Sign in successful: {signInResult?.Name ?? "Failed"}");
        
        var failedSignIn = authService.SignIn("alice@email.com", "wrongpassword");
        Console.WriteLine($"Sign in with wrong password: {failedSignIn?.Name ?? "Failed"}");
        
        var duplicateSignUp = authService.SignUp("New User", "alice@email.com", "newpassword");
        Console.WriteLine($"Duplicate email signup: {duplicateSignUp?.Name ?? "Failed"}");
    }
    
    static async Task SimulateUserBidding(User user, Auction[] auctions, int userIndex, CancellationToken cancellationToken)
    {
        var random = new Random(userIndex * 1000); // Seed based on user index for reproducible randomness
        var bidCount = 0;
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user.Name} started bidding thread");
        
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Wait a random interval between bids (1-10 seconds)
                await Task.Delay(random.Next(1000, 10000), cancellationToken);
                
                // Pick a random auction
                var auction = auctions[random.Next(auctions.Length)];
                
                // Skip if auction is not active or user is the owner
                if (!auction.IsActive() || auction.Owner == user)
                    continue;
                
                // Calculate bid amount (current highest bid + random increment)
                var bidIncrement = random.Next(10, 100);
                var bidAmount = auction.HighestBid + bidIncrement;
                
                // Occasionally make a really high bid
                if (random.Next(100) < 10) // 10% chance
                {
                    bidAmount = auction.HighestBid + random.Next(100, 500);
                }
                
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user.Name} attempting to bid ${bidAmount:F2} on '{auction.Name}'");
                
                var success = user.AddBid(auction, bidAmount);
                if (success)
                {
                    bidCount++;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✓ {user.Name} successfully placed bid #{bidCount}!");
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✗ {user.Name}'s bid failed");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {user.Name} stopped bidding (Total bids: {bidCount})");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Error in {user.Name}'s bidding thread: {ex.Message}");
        }
    }
}
