using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BoulderMovement : MonoBehaviour
{
    BoulderManager boulderMan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan = CommandCenter.Instance.boulderManager_;
    }

    public IEnumerator ShiftBouldersSmoothly (
         int whichConfig ,
         Transform smashPosition ,
         Boulders boulders,
         SmashPosition smashPos,
         float moveDuration = 0.075f ,
         float delayBetween = 0.05f ,
         Action OnComplete = null )
    {
        CommandCenter.Instance.soundManager_.PlaySound("UI_Voice12");

        BoulderPos [] boulderPositions = boulders.boulderConfigs [whichConfig].ballPositions;
        int lastIndex = boulderPositions.Length - 1;

        // 1. Handle last boulder going to smash point
        var last = boulderPositions [lastIndex];
        if (last.TheOwner != null)
        {
            var owner = last.TheOwner;
            last.RemoveOwner();
            Vector2 finalSize = new Vector2(90f,90f);
            Sequence seq = DOTween.Sequence();

            seq.Append(owner.transform.DOMove(smashPosition.position , moveDuration)
                    .SetDelay(delayBetween)
                    .SetEase(Ease.InQuad))
                .Join(owner.GetComponent<RectTransform>().DOSizeDelta(finalSize , moveDuration));

            yield return seq.WaitForCompletion();
            owner.transform.SetParent(smashPosition);
            owner.GetComponent<RectTransform>().sizeDelta = finalSize;
            smashPos.AddOwner(owner);

            boulderMan.Boulder = owner;
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
            BoulderSelection selection = CommandCenter.Instance.boulderManager_.selection;
            newBoulder.GetComponent<Boulder>().SetBoulderType(selection.GetRandomBoulderTypeConfig().type);
            newBoulder.GetComponent<Boulder>().SetBoulderSprite(selection.GetRandomBoulderTypeConfig().boulder);

            Tween spawnTween = newBoulder.transform.DOMove(bottom.transform.position , moveDuration)
                .SetEase(Ease.InQuad);

            yield return spawnTween.WaitForCompletion();
            bottom.AddOwner(newBoulder);
        }

        yield return null;
        OnComplete?.Invoke();
    }
}
