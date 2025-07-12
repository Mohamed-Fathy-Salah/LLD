namespace SplitWise;

public abstract class SplitStrategy
{
    public abstract Dictionary<IGroupObserver, decimal> UserShareAmounts(
        Dictionary<IGroupObserver, decimal> userShares, decimal amount);
}

public class EqualSplit : SplitStrategy
{
    public override Dictionary<IGroupObserver, decimal> UserShareAmounts(
        Dictionary<IGroupObserver, decimal> userShares, decimal amount)
    {
        decimal splitedAmount = amount / userShares.Count;
        return userShares.Keys.ToDictionary(f => f, f => splitedAmount);
    }
}

public class PercentageSplit : SplitStrategy
{
    public override Dictionary<IGroupObserver, decimal> UserShareAmounts(
        Dictionary<IGroupObserver, decimal> userShares, decimal amount)
    {
        if(userShares.Values.Sum() != 1m)
            throw new Exception("shares donot add up to 100% of the amount");
        return userShares.ToDictionary(f => f.Key, f => f.Value * amount);
    }
}

public class ExactAmountSplit : SplitStrategy
{
    public override Dictionary<IGroupObserver, decimal> UserShareAmounts(
        Dictionary<IGroupObserver, decimal> userShares, decimal amount)
    {
        if(userShares.Values.Sum() != amount)
            throw new Exception("shares donot add up to 100% of the amount");
        return userShares;
    }
}

public class ShareAmountSplit : SplitStrategy
{
    public override Dictionary<IGroupObserver, decimal> UserShareAmounts(
        Dictionary<IGroupObserver, decimal> userShares, decimal amount)
    {
        var sharesCount = userShares.Values.Sum();
        return userShares.ToDictionary(f => f.Key, f => f.Value / sharesCount * amount);
    }
}

// Factory Pattern
public class SplitStrategyFactory
{
    private static readonly Lazy<SplitStrategyFactory> _instance =
        new Lazy<SplitStrategyFactory>(() => new SplitStrategyFactory());

    private readonly Lazy<EqualSplit> _equalSplit = new Lazy<EqualSplit>(() => new EqualSplit());
    private readonly Lazy<PercentageSplit> _percentageSplit = new Lazy<PercentageSplit>(() => new PercentageSplit());
    private readonly Lazy<ExactAmountSplit> _exactAmountSplit = new Lazy<ExactAmountSplit>(() => new ExactAmountSplit());
    private readonly Lazy<ShareAmountSplit> _shareAmountSplit = new Lazy<ShareAmountSplit>(() => new ShareAmountSplit());

    public static SplitStrategyFactory Instance => _instance.Value;

    private SplitStrategyFactory() { }

    public SplitStrategy GetSplitStrategy(SplitStrategyEnum strategyType)
    {
        return strategyType switch
        {
            SplitStrategyEnum.EQUAL => _equalSplit.Value,
            SplitStrategyEnum.AMOUNT => _exactAmountSplit.Value,
            SplitStrategyEnum.PERCENTAGE => _percentageSplit.Value,
            SplitStrategyEnum.SHARE => _shareAmountSplit.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(strategyType), $"Unsupported strategy: {strategyType}")
        };
    }
}
