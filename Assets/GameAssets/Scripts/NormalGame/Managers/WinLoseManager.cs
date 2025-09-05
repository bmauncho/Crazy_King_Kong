using System.Collections;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    APIManager apiMan_;
    GameplayManager gamePlayMan_;
    BoulderManager boulderMan_;
    PoolManager poolMan_;
    PayOutManager payOutMan_;
    CurrencyManager currencyMan_;
    [Header("win Game probability")]
    [Range(0f , 100f)]
    public float winRate = 25f;
    [Header("Bonus Game probability")]
    [Range(0f , 100f)]
    public float bonusGameProbability;
    public SmashedVFXPosition smashedVFXPosition;
    private void Start ()
    {
        gamePlayMan_ = CommandCenter.Instance.gamePlayManager_;
        boulderMan_ = CommandCenter.Instance.boulderManager_;
        poolMan_= CommandCenter.Instance.poolManager_;
        payOutMan_ = CommandCenter.Instance.payOutManager_;
        currencyMan_ = CommandCenter.Instance.currencyManager_;
        apiMan_ = CommandCenter.Instance.apiManager_;
    }

    public bool CanWin ()
    {
        float roll = Random.value * 100f;
        return roll <= winRate;
    }

    public bool CanShowBonusGame (bool canWin)
    {
        if (!canWin) return false; // Only trigger bonus if a win happens

        float roll = Random.value * 100f;
        return roll <= bonusGameProbability;
    }

    public void win ()
    {
        StartCoroutine(winSequence());
    }

    public IEnumerator winSequence ()
    {
        Debug.Log("Win - sequence!");
        BoulderType currentWinBoulderType = boulderMan_.GetCurrentBoulderType();
        // 1. Return or hide the winning boulder
        boulderMan_.returnBoulderToPool();
        // 2. Play any win effects (VFX, SFX, UI animations etc.)
        yield return StartCoroutine(winVfx(currentWinBoulderType));
        // 3. Add win points or rewards
        gamePlayMan_.DisableWin();
        gamePlayMan_.ResetSpins();

        if (CommandCenter.Instance.IsDemo())
        {
            Debug.Log($"current winAmount{currencyMan_.CashAmount}");
            currencyMan_.updateCashAmount(payOutMan_.GetWinAmount().ToString());
            Debug.Log($"new winAmount{currencyMan_.CashAmount}");
        } 
        else
        {
            //update winamount
            apiMan_.updateBet.SetAmountWon(payOutMan_.GetWinAmount());
            apiMan_.updateBet.UpdateTheBet();
            yield return new WaitUntil(() => apiMan_.updateBet.isUpdated);
            currencyMan_.updateCashAmount(apiMan_.updateBet.new_wallet_balance.ToString());
        }


        gamePlayMan_.DisableSpin();
        // 4. Refresh boulders (wait for completion)
        yield return StartCoroutine(boulderMan_.skip.refreshBoulders());
        // 5. Enable spin or next round
        // Coroutine ends
        yield return null;

        BonusGameUI bonusUI = gamePlayMan_.bonusGame.bonusUI;
        yield return StartCoroutine(bonusUI.showBonusGameUI());

        yield return new WaitUntil(() => !gamePlayMan_.canShowBonusGame);

        if (gamePlayMan_.canAutoSpin)
        {
            gamePlayMan_.RestartAutoSpin();
        }
    }

    public void lose ()
    {
        StartCoroutine(loseSequence(boulderMan_));
    }


    public IEnumerator loseSequence (BoulderManager boulderMan)
    {
        Debug.Log("lose - sequence!");
        boulderMan.Boulder.GetComponent<Boulder>().crack();
        gamePlayMan_.DisableWin();
        yield return new WaitUntil(() => !boulderMan_.kongAnim.IsAnimating);
        gamePlayMan_.DisableSpin();
        //Debug.Log("Disable - Spin");
        yield return null;

        if (gamePlayMan_.canAutoSpin)
        {
            gamePlayMan_.RestartAutoSpin();
        }
    }

    IEnumerator winVfx (BoulderType currntWinBoulder)
    {
        Vector3 spawnPos = smashedVFXPosition.transform.position;
        GameObject smashFx = poolMan_.GetFromPool(PoolType.SmashVfx , spawnPos , Quaternion.identity , smashedVFXPosition.transform);
        smashFx.SetActive(true);

        Animator anim = smashFx.GetComponentInChildren<Animator>();

        // Get the length of the current clip
        float clipLength = 1f;
        if (anim != null && anim.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            clipLength = anim.GetCurrentAnimatorClipInfo(0) [0].clip.length;
        }

        StartCoroutine(payOutMan_.ShowWin(currntWinBoulder));

        yield return new WaitForSeconds(clipLength + 0.1f);

        poolMan_.ReturnToPool(PoolType.SmashVfx , smashFx);
    }
}
