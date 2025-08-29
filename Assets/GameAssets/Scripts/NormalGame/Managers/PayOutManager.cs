using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BaseGamePayOutConfig
{
    public GameType gameType;
    public BasePayOutConfig [] config;
}

[System.Serializable]
public class BasePayOutConfig
{
    public BoulderType BoulderType;
    public double minMultiplier;
    public double maxMultiplier;
}

[System.Serializable]
public class BonusGamePayOutConfig
{
    public GameType gameType;
    public BonusPayOutConfig [] config;
}

[System.Serializable]
public class BonusPayOutConfig
{
    public BonusOptions BonusOptions;
    public double minMultiplier;
    public double maxMultiplier;
}
public class PayOutManager : MonoBehaviour
{
    BoulderManager boulderMan_;
    BetManager betMan_;
    public BaseGamePayOutConfig BaseConfig;
    public BonusGamePayOutConfig BonusConfig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan_ = CommandCenter.Instance.boulderManager_;
        betMan_ = CommandCenter.Instance.betManager_;
    }

    public double calculatePayOut (BoulderType Type)
    {
        // payout = Bet * (probabilites * expectedMultiplier);
        string betAmount = betMan_.betAmount;
        double Bet = 1;

        if(double.TryParse(betAmount,out double newBetAmount))
        {
            Bet = newBetAmount;
        }

        double expectedPayOut = calculateExpectedMultiplier(Type) * calculateNormalizedWeights(Type);
        double payOut =  Bet * expectedPayOut;

        return payOut;
    }

    public double calculateExpectedMultiplier (BoulderType Type)
    {
        foreach(var multiplier in BaseConfig.config)
        {
            if(Type == multiplier.BoulderType)
            {
                return ( multiplier.minMultiplier + multiplier.maxMultiplier ) / 2;
            }
        }
        return 0;
    }

    public double calculateNormalizedWeights (BoulderType Type)
    {
        BoulderSelection selection = boulderMan_.selection;
        foreach(var weight in selection.probabilityWeights)
        {
            if(Type == weight.type)
            {
                return weight.weight / selection.GetTotalWeights();
            }
        }
        return 0;
    }
}
