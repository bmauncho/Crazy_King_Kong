using UnityEngine;

public class BetManager : MonoBehaviour
{
    private string [] BetAmounts = { "0.5", "1" , "2" , "4" , "5" , "10" , "20" ,"25", "50" , "100" };
    public int betIndex = 3;
    public string betAmount = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        refresh();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void refresh ()
    {
        betAmount = BetAmounts [betIndex];
    }

    public void IncreaseBetAmount_click ()
    {
        if (betIndex < BetAmounts.Length - 1)
        {
            betIndex++;
            betAmount = BetAmounts [betIndex];
        }
    }

    public void DecreaseBetAmount_Click ()
    {
        if (betIndex > 0)
        {
            betIndex--;
            betAmount = BetAmounts [betIndex];
        }
    }

    public void IncreaseBetAmount_Hold ()
    {
        if (betIndex < BetAmounts.Length - 1)
        {
            betIndex++;
            betAmount = BetAmounts [betIndex];
        }
    }

    public void DecreaseBetAmount_Hold ()
    {
        if (betIndex > 0)
        {
            betIndex--;
            betAmount = BetAmounts [betIndex];
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
