using SplitWise;

bool allTestsPassed = true;
Console.WriteLine("Starting SplitWise application tests...");

// Test 1: Basic group creation and expense splitting (EQUAL strategy)
allTestsPassed &= await TestBasicGroupAndEqualSplit();

// Test 2: Concurrent expense addition and settlement
allTestsPassed &= await TestConcurrentExpenseAndSettlement();

// Test 3: Percentage split strategy
allTestsPassed &= await TestPercentageSplit();

// Test 4: Share amount split strategy
allTestsPassed &= await TestShareAmountSplit();

// Test 5: Exact amount split strategy
allTestsPassed &= await TestExactAmountSplit();

// Test 6: Invalid operations (should fail gracefully)
allTestsPassed &= await TestInvalidOperations();

// Test 7: test concurrent settlement
allTestsPassed &= await TestConcurrentSettlement();

Console.WriteLine("\nTest Summary:");
Console.WriteLine(allTestsPassed ? "All tests passed successfully!" : "One or more tests failed!");

async Task<bool> TestBasicGroupAndEqualSplit()
{
    try
    {
        Console.WriteLine("\nTest 1: Basic group creation and equal split");
        var user1 = new User("Alice");
        var user2 = new User("Bob");
        var user3 = new User("Charlie");

        var group = user1.CreateGroup("TestGroup", [user2, user3]);

        // Run expense creation and settlement in parallel
        var tasks = new[]
        {
                Task.Run(() =>
                {
                    var shares = new Dictionary<IGroupObserver, decimal> { { user1, 0 }, { user2, 0 }, { user3, 0 } };
                    group.AddExpense("Dinner", user1, 90m, shares, SplitStrategyEnum.EQUAL);
                }),
            };

        await Task.WhenAll(tasks);

        // Verify balances
        bool success = user1.UserBalances[user2].owesMe == 30m &&
                      user1.UserBalances[user3].owesMe == 30m &&
                      user2.UserBalances[user1].owesMe == -30m &&
                      user3.UserBalances[user1].owesMe == -30m;

        Console.WriteLine($"Test 1 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 1 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestConcurrentExpenseAndSettlement()
{
    try
    {
        Console.WriteLine("\nTest 2: Concurrent expense addition and settlement");
        var user1 = new User("Dave");
        var user2 = new User("Eve");
        var user3 = new User("Frank");

        var group = user1.CreateGroup("ConcurrentGroup", [user2, user3]);

        // Multiple threads adding expenses and settling concurrently
        var tasks = new List<Task>();
        for (int i = 0; i < 5; i++)
        {
            int expenseId = i;
            tasks.Add(Task.Run(() =>
            {
                var shares = new Dictionary<IGroupObserver, decimal> { { user1, 0 }, { user2, 0 }, { user3, 0 } };
                group.AddExpense($"Expense_{expenseId}", user1, 60m, shares, SplitStrategyEnum.EQUAL);
            }));
        }

        await Task.WhenAll(tasks);

        // Verify final balances
        bool success = user1.UserBalances[user2].owesMe == 100m && // 5 * 20 (each expense split equally)
                      user1.UserBalances[user3].owesMe == 100m &&
                      group.Expenses.Count == 5;

        Console.WriteLine($"Test 2 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 2 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestPercentageSplit()
{
    try
    {
        Console.WriteLine("\nTest 3: Percentage split strategy");
        var user1 = new User("Grace");
        var user2 = new User("Heidi");
        var user3 = new User("Ivan");

        var group = user1.CreateGroup("PercentageGroup", [user2, user3]);

        var tasks = new[]
        {
                Task.Run(() =>
                {
                    var shares = new Dictionary<IGroupObserver, decimal>
                    {
                        { user1, .5m }, // 50%
                        { user2, .3m }, // 30%
                        { user3, .2m }  // 20%
                    };
                    group.AddExpense("Trip", user1, 1000m, shares, SplitStrategyEnum.PERCENTAGE);
                }),
            };

        await Task.WhenAll(tasks);

        bool success = user1.UserBalances[user2].owesMe == 300m &&
                      user1.UserBalances[user3].owesMe == 200m;

        Console.WriteLine($"Test 3 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 3 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestShareAmountSplit()
{
    try
    {
        Console.WriteLine("\nTest 4: Share amount split strategy");
        var user1 = new User("Judy");
        var user2 = new User("Karl");
        var user3 = new User("Lisa");

        var group = user1.CreateGroup("ShareGroup", [user2, user3]);

        var tasks = new[]
        {
                Task.Run(() =>
                {
                    var shares = new Dictionary<IGroupObserver, decimal>
                    {
                        { user1, 2 }, // 2 shares
                        { user2, 2 }, // 2 shares
                        { user3, 1 }  // 1 share
                    };
                    group.AddExpense("Party", user1, 100m, shares, SplitStrategyEnum.SHARE);
                }),
            };

        await Task.WhenAll(tasks);

        bool success = user1.UserBalances[user2].owesMe == 40m && // (2/5) * 100
                      user1.UserBalances[user3].owesMe == 20m;   // (1/5) * 100

        Console.WriteLine($"Test 4 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 4 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestExactAmountSplit()
{
    try
    {
        Console.WriteLine("\nTest 5: Exact amount split strategy");
        var user1 = new User("Mike");
        var user2 = new User("Nancy");
        var user3 = new User("Oscar");

        var group = user1.CreateGroup("ExactGroup", [user2, user3]);

        var tasks = new[]
        {
                Task.Run(() =>
                {
                    var shares = new Dictionary<IGroupObserver, decimal>
                    {
                        { user1, 50m },
                        { user2, 30m },
                        { user3, 20m }
                    };
                    group.AddExpense("Concert", user1, 100m, shares, SplitStrategyEnum.AMOUNT);
                }),
            };

        await Task.WhenAll(tasks);

        bool success = user1.UserBalances[user2].owesMe == 30m &&
                      user1.UserBalances[user3].owesMe == 20m;

        Console.WriteLine($"Test 5 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 5 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestInvalidOperations()
{
    try
    {
        Console.WriteLine("\nTest 6: Invalid operations handling");
        var user1 = new User("Paul");
        var user2 = new User("Quinn");
        var user3 = new User("Rachel");

        var group = user1.CreateGroup("InvalidOpsGroup", [user2]);

        var tasks = new[]
        {
                Task.Run(() =>
                {
                    try
                    {
                        // Try adding expense with non-member
                        var shares = new Dictionary<IGroupObserver, decimal> { { user3, 0 } };
                        group.AddExpense("Invalid", user1, 100m, shares, SplitStrategyEnum.EQUAL);
                        return false;
                    }
                    catch (ArgumentException)
                    {
                        return true;
                    }
                }),
                Task.Run(() =>
                {
                    try
                    {
                        // Try settling non-existent expense
                        var fakeExpense = new Expense("Fake", user1, 100m, new Dictionary<IGroupObserver, decimal>());
                        return !group.TrySettle(user2, fakeExpense);
                    }
                    catch
                    {
                        return false;
                    }
                })
            };

        var results = await Task.WhenAll(tasks);
        bool success = results.All(r => r);

        Console.WriteLine($"Test 6 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 6 FAILED: {ex.Message}");
        return false;
    }
}

async Task<bool> TestConcurrentSettlement()
{
    try
    {
        Console.WriteLine("\nTest 7: Concurrent settlement after multiple expenses");
        var user1 = new User("Sam");
        var user2 = new User("Tina");
        var user3 = new User("Uma");

        var group = user1.CreateGroup("ConcurrentSettleGroup", [user2, user3]);

        // Add multiple expenses first
        var expenseTasks = new List<Task>();
        for (int i = 0; i < 3; i++)
        {
            int expenseId = i;
            expenseTasks.Add(Task.Run(() =>
            {
                var shares = new Dictionary<IGroupObserver, decimal> { { user1, 0 }, { user2, 0 }, { user3, 0 } };
                group.AddExpense($"Expense_{expenseId}", user1, 60m, shares, SplitStrategyEnum.EQUAL);
            }));
        }

        await Task.WhenAll(expenseTasks);

        // Then perform concurrent settlements
        var settlementTasks = new List<Task>();
        for (int i = 0; i < 3; i++)
        {
            settlementTasks.Add(Task.Run(() => user2.SettleBalances(user1)));
            settlementTasks.Add(Task.Run(() => user3.SettleBalances(user1)));
        }

        await Task.WhenAll(settlementTasks);

        // Verify final balances after settlement
        bool success = user1.UserBalances[user2].owesMe == 0m && // 3 * 20 = 60, settled to 0
                      user1.UserBalances[user3].owesMe == 0m &&
                      user2.UserBalances[user1].owesMe == 0m &&
                      user3.UserBalances[user1].owesMe == 0m &&
                      group.Expenses.Count == 3;

        Console.WriteLine($"Test 7 {(success ? "PASSED" : "FAILED")}");
        return success;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Test 7 FAILED: {ex.Message}");
        return false;
    }
}
