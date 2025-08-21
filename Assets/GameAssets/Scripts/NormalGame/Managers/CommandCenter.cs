using UnityEngine;
[System.Serializable]
public enum GameMode
{
    Demo,
    Live,
}
public class CommandCenter : MonoBehaviour
{
    public static CommandCenter Instance { get; private set; }
    public GameMode gameMode; // Live or Demo
    public PoolManager poolManager_;
    public TextManager textManager_;
    public CurrencyManager currencyManager_;
    public BetManager betManager_;
    public GameplayManager gamePlayManager_;
    public BoulderManager boulderManager_;

    private void Awake ()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("This is extra!");
            Destroy(gameObject); // Ensures only one instance exists
        }
        else
        {
            Instance = this;
        }

        if (GameManager.Instance)
        {
            SetUp();
            CheckifGameIsReady();
        }
    }

    void SetUp ()
    {
        gameMode = GameManager.Instance.IsDemo() ? GameMode.Demo : GameMode.Live;
        bool isDemo = GameManager.Instance.IsDemo() ? true : false;
    }

    void CheckifGameIsReady ()
    {
        Debug.Log("IsReady!");
    }

    public void SetGameMode ( GameMode mode )
    {
        gameMode = mode;
    }

    public bool IsDemo ()
    {
        return gameMode == GameMode.Demo;
    }

    private void OnDestroy ()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
