using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BonusGameWinUI : MonoBehaviour
{
    TextManager textMan_;
    PayOutManager payOutMan_;
    APIManager apiMan_;
    CurrencyManager currencyMan_;
    public Image BigWin;
    public double winAmount;
    public TMP_Text bonusWinText;
    public GameObject Content;
    public GameObject winUIContent;
    [SerializeField] private float duration = 2f; // duration of the animation in seconds
    private Sequence bounceSequence;
    private Sequence hideSequence;
    double bonus_WinAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textMan_ = CommandCenter.Instance.textManager_;
        payOutMan_ = CommandCenter.Instance.payOutManager_;
        apiMan_ = CommandCenter.Instance.apiManager_;
        currencyMan_ = CommandCenter.Instance.currencyManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWinUi ()
    {
        Content.SetActive(true);
    }

    public void HideWinUI ()
    {
        Content.SetActive(false);
    }

    public void setWinAmount(double Amount )
    {
        winAmount = Amount;
    }

    public IEnumerator showWinAmount (BonusOptions bonusOptions)
    {
        Debug.Log("BonusPayOut!");
        ShowWinUi();

        var canvasGroup = winUIContent.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;

        double currentAmount = 0;
        float elapsed = 0;
        double bonusWinAmount = 0;
        if (CommandCenter.Instance.IsDemo())
        {
            bonusWinAmount = payOutMan_.calculateBonusPayout(bonusOptions);
        }
        else
        {
            bonusWinAmount = apiMan_.bonusApi.response.win_amount;
        }

        bonus_WinAmount = bonusWinAmount;

        setWinAmount(bonusWinAmount);

        StartCoroutine(BounceWinUi());

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            double t = elapsed / duration;
            currentAmount = Mathf.Lerp(0f , (float)winAmount , (float)t);

            string CASHAMOUNT = currentAmount.ToString();
            CASHAMOUNT = NumberFormatter.FormatString(CASHAMOUNT , 2,false);
            textMan_.refreshBonusWinUIText(CASHAMOUNT , bonusWinText);
            yield return null;
        }

        bounceSequence?.Kill();
        winUIContent.transform.localScale = Vector3.one;

        // Ensure it ends exactly at winAmount
        string finalAmount = winAmount.ToString();
        finalAmount = NumberFormatter.FormatString(finalAmount , 2,false);
        textMan_.refreshBonusWinUIText(finalAmount , bonusWinText);

        yield return null;

        bounceSequence = null;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(hideWinAmount());

    }

    IEnumerator BounceWinUi ()
    {
        bounceSequence = DOTween.Sequence();
        var ui = winUIContent.transform;
        bounceSequence.Append(ui.DOScale(1.2f , .5f))
            .Append(ui.DOScale(1f , .5f));

        bounceSequence.SetLoops(-1);
        yield return null;
    }

    public IEnumerator hideWinAmount ()
    {
        var ui = winUIContent.transform;
        var canvasGroup = winUIContent.GetComponent<CanvasGroup>();
        hideSequence = DOTween.Sequence();

        hideSequence.Append(canvasGroup.DOFade(0 , 1f))
            .Join(ui.DOScale(1.3f , 1f));

        yield return hideSequence.WaitForCompletion();
        setWinAmount(0);
        yield return null;
        hideSequence = null;

        if (CommandCenter.Instance.IsDemo())
        {
            currencyMan_.updateCashAmount(bonus_WinAmount.ToString());
        }
        else
        {
            apiMan_.updateBet.SetAmountWon(apiMan_.bonusApi.response.win_amount);
            apiMan_.updateBet.UpdateTheBet();
            yield return new WaitUntil(() => apiMan_.updateBet.isUpdated);
            currencyMan_.updateCashAmount(apiMan_.updateBet.new_wallet_balance.ToString());
        }

        HideWinUI();
    }

    [ContextMenu("Test")]
    public void Test ()
    {
        StartCoroutine(showWinAmount(BonusOptions.silver));
    }

}
