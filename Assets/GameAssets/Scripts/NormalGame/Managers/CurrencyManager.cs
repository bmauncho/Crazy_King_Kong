using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    TextManager textManager_;
    public UserInfo UserInfo;
    public double CashAmount;
    public double winAmount;
    public double cumilativeWinAMount;
    TMP_Text walletAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        walletAmountText = UserInfo.walletAmount;
        textManager_= CommandCenter.Instance.textManager_;
        if (CommandCenter.Instance)
        {
            if (CommandCenter.Instance.gameMode == GameMode.Demo)
            {
                CashAmount = 2000;
                string CASHAMOUNT = CashAmount.ToString();
                CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2);
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
                CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2);
                textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
            }
        }
    }

    public string GetTotalWinAmount ()
    {
        winAmount = 0;
        if (CommandCenter.Instance.IsDemo())
        {
            cumilativeWinAMount += winAmount;
        }
        else
        {
            cumilativeWinAMount = winAmount;
        }

        return cumilativeWinAMount.ToString("N2" , CultureInfo.CurrentCulture); ;
    }

    public IEnumerator Bet ()
    {
        if (CommandCenter.Instance.IsDemo())
        {
            string betAmount = "0";

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
        CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2);
        textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
        yield return null;
    }


    public void CollectWinnings ()
    {
        if (CommandCenter.Instance.IsDemo())
        {
            string totalWininings = GetTotalWinAmount();
            if (double.TryParse(totalWininings , out double winnings))
            {
                CashAmount += winnings;
            }
        }

        string CASHAMOUNT = CashAmount.ToString();
        CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2);
        textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
    }

    public void updateCashOutWinings ()
    {
        string totalWininings = "0";
        if (double.TryParse(totalWininings , out double winnings))
        {
            CashAmount = winnings;
        }
        string CASHAMOUNT = CashAmount.ToString();
        CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2);
        textManager_.refreshWalletText(CASHAMOUNT , walletAmountText);
    }

    public bool IsMoneyDepleted ()
    {
        return CashAmount < 0;
    }
}
