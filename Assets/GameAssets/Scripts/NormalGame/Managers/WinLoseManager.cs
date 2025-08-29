using System.Collections;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{
    GameplayManager gamePlayMan_;
    BoulderManager boulderMan_;
    PoolManager poolMan_;
    PayOutManager payOutMan_;
    CurrencyManager currencyMan_;
    [Range(0f , 100f)]
    public float winRate = 25f;
    public SmashedVFXPosition smashedVFXPosition;
    private void Start ()
    {
        gamePlayMan_ = CommandCenter.Instance.gamePlayManager_;
        boulderMan_ = CommandCenter.Instance.boulderManager_;
        poolMan_= CommandCenter.Instance.poolManager_;
        payOutMan_ = CommandCenter.Instance.payOutManager_;
        currencyMan_ = CommandCenter.Instance.currencyManager_;
    }

    public bool CanWin ()
    {
        float roll = Random.value * 100f; // Random.value returns 0.0 to 1.0
        return roll <= winRate;
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
        currencyMan_.CollectWinnings(payOutMan_.GetWinAmount().ToString());
        // 4. Refresh boulders (wait for completion)
        yield return StartCoroutine(boulderMan_.skip.refreshBoulders());
        // 5. Enable spin or next round
        gamePlayMan_.DisableSpin();
        // Coroutine ends
        yield return null;

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

        yield return new WaitForSeconds(clipLength + 0.1f);

        poolMan_.ReturnToPool(PoolType.SmashVfx , smashFx);

        StartCoroutine(payOutMan_.ShowWin(currntWinBoulder));
    }
}
