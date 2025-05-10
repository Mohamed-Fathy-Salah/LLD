using VendingMachine;

class Program
{
    static void PrintProducts(VendingMachine.VendingMachine machine)
    {
        Console.WriteLine("Available Coffees:");
        foreach (var p in machine.Repository.GetProducts())
            Console.WriteLine($"ID: {p.ID}, Name: {p.Name}, Price: {p.PriceInCents / 100.0}$");
    }

    static void PrintIngredients(VendingMachine.VendingMachine machine)
    {
        Console.WriteLine("Available Ingredients:");
        foreach (var p in machine.Repository.GetIngredients())
            Console.WriteLine($"ID: {p.ID}, Name: {p.Name}, Price: {p.PriceInCents / 100.0}$, Quantity: {p.Quantity}");
    }

    static void Main()
    {
        var machine = new VendingMachine.VendingMachine(amountInCents: 10000);
        Console.WriteLine("Coffee Machine Ready.");

        while (true)
        {
            var actions = machine.GetAllowedActions();
            Console.WriteLine("\nSelect an action:");

            for (int i = 0; i < actions.Length; i++)
                Console.WriteLine($"{i + 1}. {actions[i]}");

            Console.Write("> ");
            if (!int.TryParse(Console.ReadLine(), out int choice) ||
                choice < 1 || choice > actions.Length)
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            var action = actions[choice - 1];

            try
            {
                switch (action)
                {
                    case "Start":
                        machine.Start(); break;

                    case "OpenMachine":
                        machine.OpenMachine(); break;

                    case "Done":
                        machine.Done(); break;

                    case "Cancel":
                        machine.Cancel(); break;

                    case "InsertMoney":
                        Console.Write("Amount in cents: ");
                        machine.InsertMoney(int.Parse(Console.ReadLine()!)); break;

                    case "CollectMoney":
                        Console.Write("Amount to collect: ");
                        machine.CollectMoney(int.Parse(Console.ReadLine()!)); break;

                    case "AddProduct":
                        PrintProducts(machine);
                        Console.Write("Product ID: ");
                        int pid = int.Parse(Console.ReadLine()!);
                        Console.Write("Quantity: ");
                        int qty = int.Parse(Console.ReadLine()!);
                        machine.AddProduct(pid, qty); break;

                    case "AddNewIngredient":
                        PrintIngredients(machine);
                        Console.Write("Name: ");
                        string ingredientName = Console.ReadLine()!;
                        Console.Write("Price in cents: ");
                        int ingredientPrice = int.Parse(Console.ReadLine()!);
                        int ingredientId = machine.AddNewIngredient(ingredientName, ingredientPrice);
                        Console.WriteLine($"Added with ID: {ingredientId}"); break;

                    case "AddIngredient":
                        PrintIngredients(machine);
                        Console.Write("ingredientId ID: ");
                        int iid = int.Parse(Console.ReadLine()!);
                        Console.Write("Quantity: ");
                        int iqty = int.Parse(Console.ReadLine()!);
                        machine.AddIngredient(iid, iqty); break;

                    case "AddNewProduct":
                        {
                            Console.Write("Enter product name: ");
                            string productName = Console.ReadLine()!;

                            Console.Write("Enter fee percentage (e.g., 2 for 2x): ");
                            double fee = double.Parse(Console.ReadLine()!);

                            var steps = new List<Step>();

                            while (true)
                            {
                                Console.Write("Enter step name (or 'done' to finish): ");
                                string stepName = Console.ReadLine()!;
                                if (stepName.Equals("done", StringComparison.OrdinalIgnoreCase))
                                    break;

                                var ingredientQuantities = new Dictionary<int, int>();
                                while (true)
                                {
                                    Console.Write("Enter ingredient ID (or 'done' to finish this step): ");
                                    string input = Console.ReadLine()!;
                                    if (input.Equals("done", StringComparison.OrdinalIgnoreCase))
                                        break;

                                    int ingId = int.Parse(input);
                                    Console.Write("Enter quantity: ");
                                    int quantity = int.Parse(Console.ReadLine()!);

                                    ingredientQuantities[ingId] = quantity;
                                }

                                steps.Add(new Step(stepName, ingredientQuantities));
                            }

                            int productId = machine.Repository.AddNewProduct(productName, steps.ToArray(), fee);
                            Console.WriteLine($"Product added with ID: {productId}");
                            break;
                        }

                    case "RemoveProduct":
                        PrintProducts(machine);
                        Console.Write("Product ID: ");
                        machine.RemoveProduct(int.Parse(Console.ReadLine()!)); break;

                    default:
                        Console.WriteLine("Unsupported action.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
