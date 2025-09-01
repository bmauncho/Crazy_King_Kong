using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameOption : MonoBehaviour
{
    public GameObject BonusItem;
    public GameObject BreakAnim;
    public GameObject BonusReward;
    [Header("Glow Settings")]
    public GameObject GlowAnim;
    public Image GlowImage;
    private Sequence glowSequence;
    public float duration = 1.0f;

    [Header("Reward Settings")]
    public GameObject TheOwner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resetBonusOption();
        StartCoroutine(Glow());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Glow ()
    {
        while (isActiveAndEnabled)
        {
            GlowImage.fillAmount = 0;
            Color c = GlowImage.color;
            c.a = 1;
            GlowImage.color = c;

            glowSequence = DOTween.Sequence();
            glowSequence.Append(GlowImage.DOFillAmount(1 ,duration))
                        .Join(GlowImage.DOFade(0 ,duration));

            yield return glowSequence.WaitForCompletion();
        }
    }

    public void AddOwner ( GameObject owner )
    {
        if (TheOwner != null) return;

        TheOwner = owner;
    }

    public void RemoveOwner ()
    {
        if (TheOwner == null) return;
        TheOwner = null;
    }

    void OnDisable ()
    {
       stopGlow();
    }

    public void stopGlow ()
    {
        glowSequence?.Kill();
    }

    public IEnumerator BreakAnimSequence ()
    {
        BreakAnim.SetActive(true);
        Animator anim = BreakAnim.GetComponentInChildren<Animator>();
        anim.Rebind();

        if (anim != null)
        {
            BonusItem.SetActive(false);
            GlowAnim.SetActive(false);

            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

            BreakAnim.SetActive(false);
        }
        yield return new WaitForSeconds(3f);
    }

    public void selectBonus ()
    {
        BonusGame bonusGame = CommandCenter.Instance.gamePlayManager_.bonusGame;
        bonusGame.bonusUI.selectOption(this);
        bonusGame.bonusUI.bonusSequence();
    }

    public void resetBonusOption ()
    {
        BonusItem.SetActive(true);
        GlowAnim.SetActive(true);
    }
}
