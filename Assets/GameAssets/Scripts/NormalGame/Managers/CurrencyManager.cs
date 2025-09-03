using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    BetManager betManager;
    TextManager textManager_;
    public UserInfo UserInfo;
    public double CashAmount;
    public double winAmount;
    TMP_Text walletAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        betManager = CommandCenter.Instance.betManager_;
        walletAmountText = UserInfo.walletAmount;
        textManager_= CommandCenter.Instance.textManager_;
        if (CommandCenter.Instance)
        {
            if (CommandCenter.Instance.gameMode == GameMode.Demo)
            {
                CashAmount = 2000;
                string CASHAMOUNT = CashAmount.ToString();
                CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2, true);
                textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
            }
            else
            {
                string cashamount = "0";
                if (double.TryParse(cashamount , out double amount))
                {
                    CashAmount += amount;
                }

                string CASHAMOUNT = CashAmount.ToString();
                CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2, true);
                textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
            }
        }
    }

    public IEnumerator Bet ()
    {
        if (CommandCenter.Instance.IsDemo())
        {
            string betAmount = betManager.betAmount;

            if (double.TryParse(betAmount , out double bet))
            {
                CashAmount -= bet;
            }
            else
            {
                Debug.LogWarning($"Invalid bet amount: {betAmount}");
            }
        }
        else
        {
            //PlaceBet
        }
        string CASHAMOUNT = CashAmount.ToString();
        CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2, true);
        textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
        yield return null;
    }


    public void updateCashAmount (string Amount)
    {
        string totalWininings = Amount;

        if (double.TryParse(totalWininings , out double winnings))
        {
            CashAmount += winnings;
        }

        if(CashAmount <= 0)
        {
            CashAmount = 0;
        }

        string CASHAMOUNT = CashAmount.ToString();
        CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2, true);
        textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
    }

    public bool IsMoneyDepleted ()
    {
        string amount = betManager.betAmount;
        double betAmount = 0;
        if (double.TryParse(amount , out double newbetAmount))
        {
            betAmount = newbetAmount;
        }
        return CashAmount < 0  || betAmount>= CashAmount;
    }
}
