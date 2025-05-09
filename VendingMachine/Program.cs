class Program
{
    static void PrintProducts(VendingMachine.VendingMachine machine)
    {
        Console.WriteLine("Available Products:");
        foreach (var p in machine.GetProducts())
            Console.WriteLine($"ID: {p.ID}, Name: {p.Name}, Price: {p.PriceInCents}¢, Qty: {p.Quantity}");
    }

    static void Main()
    {
        var machine = new VendingMachine.VendingMachine(amountInCents: 10000);
        Console.WriteLine("Vending Machine Ready.");

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

                    case "AddNewProduct":
                        PrintProducts(machine);
                        Console.Write("Name: ");
                        string name = Console.ReadLine()!;
                        Console.Write("Price in cents: ");
                        int price = int.Parse(Console.ReadLine()!);
                        int newId = machine.AddNewProduct(name, price);
                        Console.WriteLine($"Added with ID: {newId}"); break;

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
