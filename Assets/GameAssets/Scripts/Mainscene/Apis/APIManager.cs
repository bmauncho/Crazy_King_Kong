using TMPro;
using UnityEngine;

public class APIManager : MonoBehaviour
{
    public const string BaseURL = "https://b.api.ibibe.africa";
    [SerializeField] private string Player_Id = string.Empty;
    [SerializeField] private string Game_Id = string.Empty;
    [SerializeField] private string Client_id = string.Empty;
    [SerializeField] private string CashAmount = string.Empty;
    [SerializeField] private string BetId = string.Empty;
    [SerializeField] private string BetAmount = string.Empty;
    public PlaceBet placeBet;
    public UpdateBet updateBet;
    public TMP_Text transactiontext;
    public TMP_Text demoText;
    private void Awake ()
    {
        SetUp();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.AddTransactionText(transactiontext);
        }

        if (CommandCenter.Instance.IsDemo())
        {
            demoText.gameObject.SetActive(true);
            transactiontext.text = "3.207.84/8bc802c";
        }
        else
        {
            demoText.gameObject.SetActive(false);
            transactiontext.text = "No.";
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }

    public void SetUp ()
    {
        Player_Id = GameManager.Instance.GetPlayerId();
        Game_Id = GameManager.Instance.GetGameId();
        Client_id = GameManager.Instance.GetClientId();
        CashAmount = GameManager.Instance.GetCashAmount();
        //Debug.Log("SetUpDone!");
    }

    public string SetBetId ()
    {
        string betId = ConfigMan.Instance.GetBetId();
        BetId = betId;
        return BetId;
    }

    public void SetBetAmount ( string amount )
    {
        BetAmount = amount;
    }

    public string GetPlayerId ()
    {
        return Player_Id;
    }

    public string GetGameId ()
    {
        return Game_Id;
    }

    public string GetClientId ()
    {
        return Client_id;
    }

    public string GetBetAmountValue ()
    {
        return BetAmount;
    }

    public string GetBetId ()
    {
        return BetId;
    }
    public string GetCashAmount ()
    {
        return CashAmount;
    }

    public string GetBaseUrl ()
    {
        return BaseURL;
    }
}
