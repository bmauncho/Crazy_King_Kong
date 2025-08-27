using System.Collections;
using UnityEngine;

public class SmashBoulder : MonoBehaviour
{
    BoulderManager boulderMan;
    public bool canSmash = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan = CommandCenter.Instance.boulderManager_;
    }


    public IEnumerator smash_Boulder ()
    {
        canSmash = false;
        if (canSmash)
        {
            boulderMan.kongAnim.playwinAnim();
        }
        else
        {
            boulderMan.kongAnim.playLoseAnim();
        }
        yield return null;
    }


}
