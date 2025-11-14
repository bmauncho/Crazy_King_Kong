using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    APIManager apiMan_;
    WinLoseManager winLoseMan_;
    CurrencyManager currencyMan_;
    public SettingsUI settingUi;
    public GameRules gameRules;
    public bool canWin = false;
    public bool canSpin = false;
    public bool canSkip = false;
    public bool canAutoSpin = false;
    public bool canShowBonusGame = false;

    public ButtonController [] buttons;
    private Coroutine autoSpin;
    public double spins;

    public BonusGame bonusGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winLoseMan_ = CommandCenter.Instance.winLoseManager_;
        currencyMan_ = CommandCenter.Instance.currencyManager_;
        apiMan_ = CommandCenter.Instance.apiManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spin ()
    {
        if (canShowBonusGame)
        {
            Debug.LogWarning("Bonus game in play unable to spin ");
            return;
        }

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

        if(currencyMan_.IsMoneyDepleted())
        {
            PromptManager.Instance.ShowErrorPrompt("Error 404" , "Insufficient balance!");
            //Debug.Log("Insufficient balance!");
            //promt
            return;
        }

        if (!canSpin)
        {
            EnableSpin();
        }

        if (CommandCenter.Instance.IsDemo())
        {
            IncreaseSpins();
            canWin = winLoseMan_.CanWin();
            canShowBonusGame = winLoseMan_.CanShowBonusGame(canWin);
        }

        StartCoroutine(smash());
    }

    IEnumerator smash ()
    {
        yield return StartCoroutine(currencyMan_.Bet());

        if (!CommandCenter.Instance.IsDemo())
        {
            apiMan_.boulderCrushAPI.crushBoulder();
            yield return new WaitUntil(() => apiMan_.boulderCrushAPI.IsDone);
            canWin = apiMan_.boulderCrushAPI.response.boulder_broken;
            canShowBonusGame = apiMan_.boulderCrushAPI.response.bonus_triggered;
        }

        yield return null;

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
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice3");
            StartAutospinSequence();
            DisableButtons();
        }
        else
        {
            canAutoSpin = false;
            StopAutoSpinSequence();
            CommandCenter.Instance.soundManager_.PlaySound("UI_Voice3");
            EnableButtons();
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
        if (!canAutoSpin)
        {
            autoSpin = null;
            yield break;
        }
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

    public void resetBonusGame ()
    {
        canShowBonusGame = false;
    }
}
