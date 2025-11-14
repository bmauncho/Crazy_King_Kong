using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class BonusApiRequest
{
    public string client_id;
    public string game_id;
    public string player_id;
    public string bet_id;
    public double bet_amount;
    public string stone_type;
}

[System.Serializable]
public class BonusApiResponse
{
    public string status;
    public string message;
    public string boulder_type;
    public string multiplier;
    public double win_amount;
}
public class BonusApi : MonoBehaviour
{
    APIManager apiMan_;
    GameplayManager gameplayMan_;
    [Header("Response")]
    public BonusApiResponse response;
    public bool IsDone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        apiMan_ = CommandCenter.Instance.apiManager_;
        gameplayMan_ = CommandCenter.Instance.gamePlayManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BonusGame ()
    {
        IsDone = false;
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new FloatTrimConverter());
        settings.Formatting = Formatting.Indented;
        apiMan_.SetBetId();
        string betAmountString = apiMan_.GetBetAmountValue();
        float betAmount = 0;

        if (float.TryParse(betAmountString , out float newBetAMount))
        {
            betAmount = newBetAMount;
        }
        
        string optionType = gameplayMan_.bonusGame.bonusUI.selectedOption.TheOwner.GetComponent<BonusReward>().Option.ToString();

        BonusApiRequest request = new BonusApiRequest
        {
            client_id = apiMan_.GetClientId() ,
            game_id = apiMan_.GetGameId() ,
            player_id = apiMan_.GetPlayerId() ,
            bet_id = apiMan_.GetBetId() ,
            bet_amount = newBetAMount ,
            stone_type = optionType ,
        };


        string jsonData = JsonConvert.SerializeObject(request , settings);
        //Debug.Log($"Bonus api request:{jsonData}");
        StartCoroutine(bonusApi(jsonData));
    }

    private IEnumerator bonusApi ( string jsonData )
    {
        string UpdatedUrl = "https://b.games.ibibe";
        string originalUrl = apiMan_.GetBaseUrl();
        string ApiUrl = UpdatedUrl + "/bonus/crazykingkong";
        using (UnityWebRequest webRequest = new UnityWebRequest(ApiUrl , "POST"))
        {
            byte [] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type" , "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                IsDone = true;
                PromptManager.Instance.ShowErrorPrompt(webRequest.result.ToString() , webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                response = JsonConvert.DeserializeObject<BonusApiResponse>(responseText);
                var parsedJson = JToken.Parse(responseText);
                string formattedOutput = JsonConvert.SerializeObject(parsedJson , Formatting.Indented);
                //Debug.Log($"Bonus api response:{formattedOutput}");
                IsDone = true;
            }
        }
    }
}
