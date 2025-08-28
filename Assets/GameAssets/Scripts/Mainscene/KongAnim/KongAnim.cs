using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KongAnimType
{
    Idle,
    ThumpBoth,
    ThumpSingle,
    WinBoth,
    WinSingle,
}

[System.Serializable]
public class KongAnimation
{
    public string name;
    public KongAnimType type;
    public Animator anim;
}

public class KongAnim : MonoBehaviour
{
    public KongAnimType currentAimType;
    public KongAnimation [] KongAnimations;

    [Header("Repeat Config")]
    public int maxRepeat = 2;

    private int lastWinIndex = -1;
    private int winRepeatCount = 0;

    private int lastLoseIndex = -1;
    private int loseRepeatCount = 0;

    private KongAnimType [] winTypes = { KongAnimType.WinBoth , KongAnimType.WinSingle };
    private KongAnimType [] loseTypes = { KongAnimType.ThumpBoth , KongAnimType.ThumpSingle };

    public bool IsAnimating = false;
    public IEnumerator PlayKongAnim ( KongAnimType type )
    {
        if (currentAimType == type)
            yield break;
        IsAnimating = true;
        var anim = GetKongAnim(type);

        foreach (var kAnim in KongAnimations)
        {
            bool isActive = ( kAnim.type == type );
            kAnim.anim.gameObject.SetActive(isActive);
        }

        if (anim?.anim != null)
        {
            currentAimType = type;
        }

        SmashEvent smashEvent = anim.anim.GetComponent<SmashEvent>();
        if (smashEvent != null)
        {
            yield return new WaitUntil(() => anim.anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && smashEvent.IsComplete);
            smashEvent.IsComplete = false;
        }
        IsAnimating = false;
    }

    public KongAnimation GetKongAnim ( KongAnimType type )
    {
        foreach (var anim in KongAnimations)
        {
            if (anim.type == type)
                return anim;
        }
        return null;
    }

    public void playwinAnim ()
    {
        int index = GetNonRepeatingIndex(winTypes.Length , ref lastWinIndex , ref winRepeatCount);
        KongAnimType winType = winTypes [index];
        StartCoroutine(PlayKongAnim(winType));
    }

    public void playLoseAnim ()
    {
        int index = GetNonRepeatingIndex(loseTypes.Length , ref lastLoseIndex , ref loseRepeatCount);
        KongAnimType loseType = loseTypes [index];
        StartCoroutine(PlayKongAnim(loseType));
    }

    public void playNormalAnim ()
    {
        StartCoroutine(PlayKongAnim(KongAnimType.Idle));
    }

    private int GetNonRepeatingIndex ( int count , ref int lastIndex , ref int repeatCount )
    {
        int index;

        if (lastIndex == -1)
        {
            index = Random.Range(0 , count);
            repeatCount = 1;
        }
        else
        {
            List<int> possibleIndices = new List<int>();

            if (repeatCount >= maxRepeat)
            {
                for (int i = 0 ; i < count ; i++)
                {
                    if (i != lastIndex)
                        possibleIndices.Add(i);
                }
            }
            else
            {
                for (int i = 0 ; i < count ; i++)
                {
                    possibleIndices.Add(i);
                }
            }

            index = possibleIndices [Random.Range(0 , possibleIndices.Count)];

            repeatCount = ( index == lastIndex ) ? repeatCount + 1 : 1;
        }

        lastIndex = index;
        return index;
    }
}
