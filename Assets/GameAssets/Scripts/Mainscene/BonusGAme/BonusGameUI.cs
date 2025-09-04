using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BonusGameUI : MonoBehaviour
{
    GameplayManager gamePlayMan_;
    APIManager apiMan_;
    TextManager textMan_;
    public GameObject content;
    public BonusGameOption selectedOption;
    public BonusGameOption [] options;
    public TMP_Text multiplierText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamePlayMan_ = CommandCenter.Instance.gamePlayManager_;
        apiMan_ = CommandCenter.Instance.apiManager_;
        textMan_ = CommandCenter.Instance.textManager_;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBonusGame ()
    {
        content.SetActive (true);
    }

    public void HideBonusGame ()
    {
        content.SetActive (false);
    }

    public IEnumerator showBonusGameUI ()
    {
        if(!gamePlayMan_.canShowBonusGame) yield break;

        gamePlayMan_.bonusGame.setUpBonusRewards ();
        
        ShowBonusGame ();

        Animator anim = content.GetComponent<Animator> ();

        anim.Play("BonusGameUI_Show");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1);

        yield return null;
    }

    public IEnumerator hideBonusGameUI (Action HideMultiplier =null)
    {
        Animator anim = content.GetComponent<Animator>();
        anim.Rebind();
        anim.Play("BonusGameUI_Hide");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        Debug.Log("Hide anim Done");
        HideMultiplier?.Invoke();
        HideBonusGame ();
        yield return null;
        Debug.Log("Show bonusreward");
    }

    public void selectOption(BonusGameOption which )
    {
        if (selectedOption != null) return;
        selectedOption = which;
    }

    public void removeOption ()
    {
        selectedOption = null;
    }

    [ContextMenu("Show - Test")]
    public void show ()
    {
        StartCoroutine(showBonusGameUI ());
    }

    [ContextMenu("Hide - Test")]
    public void hide ()
    {
        StartCoroutine(hideBonusGameUI ());
    }

    public void bonusSequence ()
    {
        StartCoroutine(BonusSequence());
    }

    IEnumerator BonusSequence ()
    {
        yield return StartCoroutine(selectedOption.BreakAnimSequence ());

        //show multiplier
        showMultiplier();
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine (hideBonusGameUI(() =>
        {
            hideMultiplier();
        }));

        Debug.Log("Show_BonusWin ");
        BonusGameWinUI bonusGameWinUI = gamePlayMan_.bonusGame.bonusWinUI;
        BonusOptions bonusOption = selectedOption.TheOwner.GetComponent<BonusReward>().Option;
        Debug.Log($"selected bonusoption - {bonusOption.ToString()}");
        removeOption();
        //win visualization
        yield return StartCoroutine(bonusGameWinUI.showWinAmount(bonusOption));
        //set is canshowbonusgame to false
        gamePlayMan_.resetBonusGame();
    }

    void showMultiplier ()
    {
        multiplierText.gameObject.SetActive(true);
        string multiplier = apiMan_.bonusApi.response.multiplier +"x";
        textMan_.refreshBonusMultiplierUIText(multiplier , multiplierText);
    }

    public void hideMultiplier ()
    {
        multiplierText.gameObject.SetActive(false);
    }
}
