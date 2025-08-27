using UnityEngine;

public class SmashEvent : MonoBehaviour
{
    public bool IsComplete = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitEffect ()
    {
        Debug.Log("play - hit effect");
        //baulder hit effect depending if its a win or a lose

        //crack effet / dust effect
        //break effect
    }


    public void ResetToIdle ()
    {
        Debug.Log("play - idle");
        IsComplete = true;
        CommandCenter.Instance.boulderManager_.kongAnim.playNormalAnim();
    }
}
