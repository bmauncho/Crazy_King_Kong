using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    WinLoseManager winLoseMan_;
    public SettingsUI settingUi;
    public GameRules gameRules;
    public bool canWin = false;
    public bool canSpin = false;
    public bool canSkip = false;
    public bool canAutoSpin = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winLoseMan_ = CommandCenter.Instance.winLoseManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spin ()
    {
        if (canSpin)
        {
            Debug.LogWarning("Already Spinning!");
            return;
        }

        if(canSkip)
        {
            Debug.LogWarning("Cannot spin while skipping!");
            return;
        }

        if (!canSpin)
        {
            EnableSpin();
        }

        canWin = winLoseMan_.CanWin();
        StartCoroutine(smash());
    }


    IEnumerator smash ()
    {
        canWin = winLoseMan_.CanWin();
        yield return new WaitForSeconds(.25f);
        CommandCenter.Instance.boulderManager_.SmashBoulder();
    }

    public void skip()
    {
        if (canSkip)
        {
            Debug.LogWarning("Already Skipping!");
            return;
        }

        if(canSpin)
        {
            Debug.LogWarning("Cannot skip while spinning!");
            return;
        }

        if (!canSkip)
        {
            EnableSkip();
            return;
        }
        //Debug.Log("Skip button clicked!");
        StartCoroutine(skip_Boulder());
    }

    IEnumerator skip_Boulder ()
    {
        yield return new WaitForSeconds(.25f);
        CommandCenter.Instance.boulderManager_.SkipBoulder();
    }

    public void ToggleSettings ()
    {
        settingUi.ToggleSettings();
    }

    public void ToggleGameRules ()
    {
        gameRules.toggleGameRules();
    }

    public void ToggleAutoSpin ()
    {
        if (!canAutoSpin)
        {
            canAutoSpin = true;
        }
        else
        {
            canAutoSpin = false;
        }
    }

    public void EnableSpin ()
    {
        canSpin = true;
    }

    public void DisableSpin ()
    {
        canSpin = false;
    }

    public void EnableSkip ()
    {
        canSkip = true;
    }

    public void DisableSkip ()
    {
        canSkip = false;
    }

    public void DisableWin ()
    {
        canWin = false;
    }
}
