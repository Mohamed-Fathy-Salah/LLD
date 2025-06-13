using System.Collections.Concurrent;

public enum CurrencyEnum
{
    EGP,
    USD,
    EUR,
}

public static class CurrencyExchage
{
    private static ConcurrentDictionary<CurrencyEnum, ConcurrentDictionary<CurrencyEnum, decimal>> _exchangeRates = new()
    {
        [CurrencyEnum.EGP] = new ConcurrentDictionary<CurrencyEnum, decimal>
        {
            [CurrencyEnum.USD] = 0.032m,
            [CurrencyEnum.EUR] = 0.029m
        },
        [CurrencyEnum.USD] = new ConcurrentDictionary<CurrencyEnum, decimal>
        {
            [CurrencyEnum.EGP] = 31.25m,
            [CurrencyEnum.EUR] = 0.91m
        },
        [CurrencyEnum.EUR] = new ConcurrentDictionary<CurrencyEnum, decimal>
        {
            [CurrencyEnum.EGP] = 34.48m,
            [CurrencyEnum.USD] = 1.10m
        }
    };

    public static void SetExchangeRate(CurrencyEnum from, CurrencyEnum to, decimal rate)
    {
        if (rate <= 0)
        {
            Console.WriteLine("Exchange rate must be greater than zero.");
            return;
        }

        _exchangeRates.AddOrUpdate(from,
            new ConcurrentDictionary<CurrencyEnum, decimal> { [to] = rate },
            (key, existing) =>
            {
                existing[to] = rate;
                return existing;
            });

        _exchangeRates.AddOrUpdate(to,
            new ConcurrentDictionary<CurrencyEnum, decimal> { [from] = 1 / rate },
            (key, existing) =>
            {
                existing[from] = 1 / rate;
                return existing;
            });
    }

    public static decimal Convert(CurrencyEnum from, CurrencyEnum to, decimal amount)
    {
        if (amount < 0)
        {
            throw new Exception("amount should be bigger than 0");
        }

        if (from == to)
            return amount;

        if (_exchangeRates.TryGetValue(from, out var rates) && rates.TryGetValue(to, out var rate))
        {
            return amount * rate;
        }

        throw new Exception($"Exchange rate from {from} to {to} not found.");
    }
}

