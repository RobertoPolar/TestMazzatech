//QUESTION 1

List<ITrade> trades = new List<ITrade>
        {
            new Trade { Value = 2000000, ClientSector = "Private" },
            new Trade { Value = 400000, ClientSector = "Public" },
            new Trade { Value = 500000, ClientSector = "Public" },
            new Trade { Value = 500000, ClientSector = "TEST" },
            new Trade { Value = 3000000, ClientSector = "Public" }
        };

var categorizer = new TradeCategorizerContext();

var tradeCategories = categorizer.CategorizeTrades(trades);

foreach (var category in tradeCategories)
{
    Console.WriteLine(category);
}

public interface ITrade
{
    double Value { get; }
    string ClientSector { get; }
}

interface ITradeCategoryStrategy
{
    string CategorizeTrade(ITrade trade);
}

class LowRiskCategory : ITradeCategoryStrategy
{
    public string CategorizeTrade(ITrade trade)
    {
        if (trade.Value < 1000000 && trade.ClientSector == "Public")
        {
            return "LOWRISK";
        }
        return null;
    }
}

class MediumRiskCategory : ITradeCategoryStrategy
{
    public string CategorizeTrade(ITrade trade)
    {
        if (trade.Value >= 1000000 && trade.ClientSector == "Public")
        {
            return "MEDIUMRISK";
        }
        return null;
    }
}

class HighRiskCategory : ITradeCategoryStrategy
{
    public string CategorizeTrade(ITrade trade)
    {
        if (trade.Value >= 1000000 && trade.ClientSector == "Private")
        {
            return "HIGHRISK";
        }
        return null;
    }
}

class TradeCategorizerContext
{
    private List<ITradeCategoryStrategy> strategies;

    public TradeCategorizerContext()
    {
        strategies = new List<ITradeCategoryStrategy>
        {
            new LowRiskCategory(),
            new MediumRiskCategory(),
            new HighRiskCategory()
        };
    }

    public List<string> CategorizeTrades(List<ITrade> trades)
    {
        List<string> tradeCategories = new List<string>();

        foreach (var trade in trades)
        {
            string category = null;
            foreach (var strategy in strategies)
            {
                category = strategy.CategorizeTrade(trade);
                if (category != null)
                {
                    break;
                }
            }

            if (category != null)
                tradeCategories.Add(category);
        }

        return tradeCategories;
    }
}

class Trade : ITrade
{
    public double Value { get; set; }
    public string ClientSector { get; set; }
}
