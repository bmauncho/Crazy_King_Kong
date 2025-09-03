using System.Collections;
using TMPro;
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
    TextManager textMan_;
    GameplayManager gameplayMan_;
    PoolManager poolMan_;
    public BaseGamePayOutConfig BaseConfig;
    public BonusGamePayOutConfig BonusConfig;

    public WinUI winUI;

    double TotalWinAmount;

    public GameObject WinUIFx_SpawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan_ = CommandCenter.Instance.boulderManager_;
        betMan_ = CommandCenter.Instance.betManager_;
        textMan_ = CommandCenter.Instance.textManager_;
        gameplayMan_ = CommandCenter.Instance.gamePlayManager_;
        poolMan_ = CommandCenter.Instance.poolManager_;
    }

    public double calculatePayOut (BoulderType Type,double spinsToCrush)
    {
        // payout = Bet * (probabilites * expectedMultiplier);
        Debug.Log($"BoulderType - {Type.ToString()}");
        string betAmount = betMan_.betAmount;

        double Bet = 1;

        if(double.TryParse(betAmount,out double newBetAmount))
        {
            Bet = newBetAmount;
        }

        double expectedPayOut = calculateExpectedMultiplier(Type) * calculateNormalizedWeights(Type);
        double payOut =  Bet * expectedPayOut;
        //Debug.Log($"Spins to crush - {spinsToCrush}");
        TotalWinAmount = payOut/spinsToCrush;
        return TotalWinAmount;
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

    public double calculateBonusPayout (BonusOptions bonusOptions)
    {
        string betAmount = betMan_.betAmount;

        double Bet = 1;

        if (double.TryParse(betAmount , out double newBetAmount))
        {
            Bet = newBetAmount;
        }

        double payOut = Bet * calculateExpectedBonusMultiplier(bonusOptions);

        return payOut;
    }

    public double calculateExpectedBonusMultiplier(BonusOptions option )
    {
        foreach(var multiplier in BonusConfig.config)
        {
            if (option == multiplier.BonusOptions)
            {
                return ( multiplier.minMultiplier + multiplier.maxMultiplier ) / 2;
            }
        }

        return 0;
    }

    public IEnumerator ShowWin (BoulderType boulderType)
    {
        winUI.ShowWinUI();
        double spinsToCrush = gameplayMan_.spins;
        TMP_Text winText = winUI.winAmount;
        string winAmount = calculatePayOut(boulderType,spinsToCrush).ToString();
        //Debug.Log("winAmount : " + winAmount);
        string payOut = NumberFormatter.FormatString(winAmount , 2, true);
        //Debug.Log(payOut);
        textMan_.refreshWinText(payOut, winText);

        GameObject winUiFx = poolMan_.GetFromPool(
            PoolType.WinUIFx ,
            WinUIFx_SpawnPos.transform.position ,
            Quaternion.identity ,
            WinUIFx_SpawnPos.transform);


        winUiFx.GetComponent<WinUIFx>().ShowWinFx(() =>
        {
            poolMan_.ReturnToPool(PoolType.WinUIFx , winUiFx);
        });

        yield return new WaitForSeconds(1f);
        HideWinUI();
    }

    public void HideWinUI ()
    {
        winUI.HideWinUI();
    }

    public double GetWinAmount ()
    {
        return TotalWinAmount;
    }

}
