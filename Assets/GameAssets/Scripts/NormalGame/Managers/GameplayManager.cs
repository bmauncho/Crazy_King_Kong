using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum BonusOptions
{
    Gold,
    Silver,
    Bronze,
}
public class GameplayManager : MonoBehaviour
{
    WinLoseManager winLoseMan_;
    CurrencyManager currencyMan_;
    public SettingsUI settingUi;
    public GameRules gameRules;
    public bool canWin = false;
    public bool canSpin = false;
    public bool canSkip = false;
    public bool canAutoSpin = false;
    public ButtonController [] buttons;
    private Coroutine autoSpin;
    public double spins;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winLoseMan_ = CommandCenter.Instance.winLoseManager_;
        currencyMan_ = CommandCenter.Instance.currencyManager_;
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
        IncreaseSpins();
        canWin = winLoseMan_.CanWin();
        StartCoroutine(smash());
    }


    IEnumerator smash ()
    {
        canWin = winLoseMan_.CanWin();
        yield return StartCoroutine(currencyMan_.Bet());
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

            StartAutospinSequence();
        }
        else
        {
            canAutoSpin = false;
            StopAutoSpinSequence();
        }
    }

    public void StartAutospinSequence ()
    {
        if (autoSpin == null)
        {
            autoSpin = StartCoroutine(AutoSpin());
        }
    }

    public void StopAutoSpinSequence ()
    {
        if (autoSpin != null)
        {
            StopCoroutine(autoSpin);
            autoSpin = null;
        }
    }

    public void RestartAutoSpin ()
    {
        if(canAutoSpin && autoSpin == null)
        {
            autoSpin = StartCoroutine(AutoSpin());
        }
    }

    public IEnumerator AutoSpin ()
    {
        yield return new WaitUntil(() => !canSkip && !canSpin);
        spin();
        yield return null;
        autoSpin = null;
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

    public void EnableButtons ()
    {
        foreach(var button in buttons)
        {
            button.isInteractable = true;
        }
    }

    public void DisableButtons ()
    {
        foreach(var button in buttons)
        {
            button.isInteractable = false;
        }
    }

    public void IncreaseSpins ()
    {
        spins++;
    }

    public void ResetSpins ()
    {
        spins = 0;
    }
}
