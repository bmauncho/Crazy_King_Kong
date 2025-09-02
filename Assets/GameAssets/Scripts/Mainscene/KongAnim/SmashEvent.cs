using UnityEngine;

public class SmashEvent : MonoBehaviour
{
    BoulderManager boulderMan;
    GameplayManager gameplayMan;
    WinLoseManager winLoseMan;
    public bool IsComplete = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boulderMan = CommandCenter.Instance.boulderManager_;
        gameplayMan = CommandCenter.Instance.gamePlayManager_;
        winLoseMan = CommandCenter.Instance.winLoseManager_;
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
        bool isWin = gameplayMan.canWin;
        if (isWin)
        {
            winLoseMan.win();
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice7");
        }
        else
        {
            winLoseMan.lose();
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice5");
        }
           
        //break effect/wineffect
    }


    public void ResetToIdle ()
    {
        //Debug.Log("play - idle");
        IsComplete = true;
        CommandCenter.Instance.boulderManager_.kongAnim.playNormalAnim();
    }
}
