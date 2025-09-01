using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BonusGameUI : MonoBehaviour
{
    GameplayManager gamePlayMan_;
    public GameObject content;
    public BonusGameOption selectedOption;
    public BonusGameOption [] options;
    private Sequence ShowSequence;
    private Sequence HideSequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamePlayMan_ = CommandCenter.Instance.gamePlayManager_;
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
        //if(!gamePlayMan_.canShowBonusGame) yield break;
        
        ShowBonusGame ();

        Animator anim = content.GetComponent<Animator> ();

        anim.Play("BonusGameUI_Show");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=1);

        yield return null;
    }

    public IEnumerator hideBonusGameUI ()
    {
        Animator anim = content.GetComponent<Animator>();
        anim.Rebind();
        anim.Play("BonusGameUI_Hide");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        removeOption();
        HideBonusGame ();
        yield return null;
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
}
