using System;
using System.Collections;
using UnityEngine;

public class SkipBoulder : MonoBehaviour
{
    BoulderManager boulderMan;
    PoolManager poolMan_;
    GameplayManager gameplayMan_;

    private void Start ()
    {
        boulderMan = CommandCenter.Instance.boulderManager_;
        poolMan_ = CommandCenter.Instance.poolManager_;
        gameplayMan_ = CommandCenter.Instance.gamePlayManager_;
    }
    public IEnumerator skip_Boulder ()
    {
        //refresh boulders
        yield return StartCoroutine(refreshBoulders(() =>
        {
            boulderMan.IsSkip = false;
            //Debug.Log("Skip IsDone");
            gameplayMan_.DisableSkip();
        }));

        yield return null;
    }

    public IEnumerator refreshBoulders ( Action OnComplete = null )
    {
        int whichConfig = boulderMan.boulders.WhichConfig();

        BoulderConfig config = boulderMan.boulders.boulderConfigs [whichConfig];

        var smashPosOwner = boulderMan.smashPos.TheOwner;

        if (smashPosOwner != null)
        {
            boulderMan.smashPos.RemoveOwner();
            poolMan_.ReturnToPool(PoolType.Boulder , smashPosOwner);
        }

        yield return StartCoroutine(boulderMan.movement.ShiftBouldersSmoothly(
            whichConfig ,
            boulderMan.smashPos.transform ,
            boulderMan.boulders ,
            boulderMan.smashPos));

        OnComplete?.Invoke();
        yield return null;
    }
}
