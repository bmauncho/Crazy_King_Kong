using System.Globalization;
using UnityEngine;

public class BetManager : MonoBehaviour
{
    TextManager textManager_;
    private string [] BetAmounts = { "0.5", "1" , "2" , "4" , "5" , "10" , "20" ,"25", "50" , "100" };
    public int betIndex = 3;
    public string betAmount = "";
    public BetAmount betAmountUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        textManager_ = CommandCenter.Instance.textManager_;
    }

    // Update is called once per frame
    void Update ()
    {

    }

    public void SetUp ()
    {
        if(CommandCenter.Instance != null && CommandCenter.Instance.gameMode == GameMode.Live)
        {
            if(ConfigMan.Instance.BetValues != null || ConfigMan.Instance.BetValues.Length > 0)
            {
                BetAmounts = ConfigMan.Instance.BetValues;
            }
        }

        refresh();
    }

    void refresh ()
    {
        betAmount = BetAmounts [betIndex];
        betAmount = PrecisionFormatter.culturedFormat(betAmount);
        textManager_.refreshBetText(betAmount , betAmountUI.Bet_Amount);
    }

    [ContextMenu("Increase Bet Amount")]
    public void IncreaseBetAmount ()
    {
        if (betIndex < BetAmounts.Length - 1)
        {
            betIndex++;
            betAmount = BetAmounts [betIndex];
            betAmountUI.Bet_Amount.text = betAmount; // Update the UI text with the new bet amount
            betAmount = PrecisionFormatter.culturedFormat (betAmount);

            textManager_.refreshBetText(betAmount , betAmountUI.Bet_Amount);
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice3");
        }
    }
    [ContextMenu("Decrease Bet Amount")]
    public void DecreaseBetAmount ()
    {
        if (betIndex > 0)
        {
            betIndex--;
            betAmount = BetAmounts [betIndex];
            betAmountUI.Bet_Amount.text = betAmount; // Update the UI text with the new bet amount

            betAmount = PrecisionFormatter.culturedFormat(betAmount);

            textManager_.refreshBetText(betAmount , betAmountUI.Bet_Amount);
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice3");
        }
    }
    public string GetBetAmount ()
    {
        return betAmount;
    }

    public bool IsHighestBetAmount ()
    {
        return betIndex >= BetAmounts.Length - 1;
    }

    public bool IsLowestBetAmount ()
    {
        return betIndex <= 0;
    }
}
