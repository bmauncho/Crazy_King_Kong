using UnityEngine;

public class SmashEvent : MonoBehaviour
{
    BoulderManager boulderMan;
    public bool IsComplete = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan = CommandCenter.Instance.boulderManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitEffect ()
    {
        //Debug.Log("play - hit effect");
        //baulder hit effect depending if its a win or a lose
        //crack effet / lose effect
        boulderMan.Boulder.GetComponent<Boulder>().crack();
        //break effect/wineffect
    }


    public void ResetToIdle ()
    {
        //Debug.Log("play - idle");
        IsComplete = true;
        CommandCenter.Instance.boulderManager_.kongAnim.playNormalAnim();
    }
}
