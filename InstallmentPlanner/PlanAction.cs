namespace InstallmentPlanner;

public abstract class PlanAction
{
    public DateTime date { get; set; }
    public Installment[] installments { get; set; }
    public abstract Task RunAsync(Plan plan);
}

public class Initiation : PlanAction
{
    public override Task RunAsync(Plan plan)
    {
        throw new NotImplementedException();
    }
}

public class Corridor : PlanAction
{
    public override Task RunAsync(Plan plan)
    {
        throw new NotImplementedException();
    }
}

public class Termination : PlanAction
{
    public override Task RunAsync(Plan plan)
    {
        throw new NotImplementedException();
    }
}

public class Securitization : PlanAction
{
    public override Task RunAsync(Plan plan)
    {
        throw new NotImplementedException();
    }
}
