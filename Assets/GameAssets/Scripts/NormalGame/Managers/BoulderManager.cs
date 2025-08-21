using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    PoolManager poolMan_;
    public SmashPosition smashPos;
    public Boulders boulders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        poolMan_ = CommandCenter.Instance.poolManager_;
        Invoke(nameof(setUpBoulders) , 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUpBoulders ()
    {
        foreach(var boulderPos in boulders.boulderConfigs)
        {
            foreach (var pos in boulderPos.ballPositions)
            {
                if (pos.TheOwner == null)
                {
                    var boulder = poolMan_.GetFromPool(
                        PoolType.Boulder ,
                        pos.transform.position ,
                        Quaternion.identity ,
                        pos.transform);

                    boulder.transform.localPosition = Vector3.zero;

                    pos.AddOwner(boulder);
                    
                }
            }
        }

        var smashBoulder = poolMan_.GetFromPool(PoolType.Boulder ,
            smashPos.transform.position ,
            Quaternion.identity ,
            smashPos.transform);

        smashBoulder.transform.localPosition = Vector3.zero;

        smashPos.AddOwner(smashBoulder);
    }

    public void SmashBoulder ()
    {
        StartCoroutine(smash_Boulder());
    }

    public IEnumerator smash_Boulder ()
    {
        //animater smashing
        //smash boulder
        //new boulder
        yield return null;
    }


    public void SkipBoulder ()
    {
        StartCoroutine(skip_Boulder());
    }

    public IEnumerator skip_Boulder ()
    {
        //refresh boulders
        yield return StartCoroutine(refreshBoulders());
        yield return null;
    }

    public IEnumerator refreshBoulders ()
    {
        int whichConfig = boulders.WhichConfig();

        BoulderConfig config = boulders.boulderConfigs [whichConfig];

        var smashPosOwner = smashPos.TheOwner;

        if (smashPosOwner != null)
        {
            smashPos.RemoveOwner();
            poolMan_.ReturnToPool(PoolType.Boulder,smashPosOwner);
        }
        yield return StartCoroutine(ShiftBouldersSmoothly(
            whichConfig,
            smashPos.transform ));

        yield return null;
    }

    public IEnumerator ShiftBouldersSmoothly (
    int whichConfig ,
    Transform smashPosition ,
    float moveDuration = 0.3f ,
    float delayBetween = 0.1f ,
    Action OnComplete = null )
    {
        BoulderPos [] boulderPositions = boulders.boulderConfigs [whichConfig].ballPositions;
        int lastIndex = boulderPositions.Length - 1;

        // 1. Handle last boulder going to smash point
        var last = boulderPositions [lastIndex];
        if (last.TheOwner != null)
        {
            var owner = last.TheOwner;
            last.RemoveOwner();
            Tween myTween = owner.transform.DOMove(smashPosition.position , moveDuration)
                .SetDelay(lastIndex * delayBetween)
                .SetEase(Ease.InQuad);
            yield return myTween.WaitForCompletion();
            owner.transform.SetParent(smashPosition);

        }

        // 2. Shift owners from top to bottom
        for (int i = lastIndex ; i > 0 ; i--)
        {
            var current = boulderPositions [i];
            var next = boulderPositions [i - 1];

            if (current.TheOwner == null && next.TheOwner != null)
            {
                var owner = next.TheOwner;
                next.RemoveOwner();
                owner.transform.SetParent(current.transform);
                Tween moveTween = owner.transform.DOMove(current.transform.position , moveDuration)
                    .SetDelay(delayBetween)
                    .SetEase(Ease.InQuad);

                yield return moveTween.WaitForCompletion();
                current.AddOwner(owner);
            }
        }

        // 3. Spawn new boulder into bottom if empty
        var bottom = boulderPositions [0];
        if (bottom.TheOwner == null)
        {
            var spawner = boulders.boulderConfigs [whichConfig].spawner;
            var newBoulder = spawner.spawnBall();
            newBoulder.transform.SetParent(bottom.transform);

            Tween spawnTween = newBoulder.transform.DOMove(bottom.transform.position , moveDuration)
                .SetEase(Ease.InQuad);

            yield return spawnTween.WaitForCompletion();
            bottom.AddOwner(newBoulder);
        }

        yield return null;
        OnComplete?.Invoke();
    }

}
