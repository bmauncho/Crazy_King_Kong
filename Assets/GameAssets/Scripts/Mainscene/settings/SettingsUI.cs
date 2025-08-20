using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public RectTransform GamePlayBtns;
    public RectTransform SettingsBtns;
    public CanvasGroup BetAmount;
    public float duration = 0.5f;

    private Tween gamePlayTween;
    private Tween settingsTween;
    private Tween betFadeTween; 

    private Vector2 originalGamePlayPos;
    private Vector2 originalSettingsPos;

    public bool IsOpen = false;
    private bool isTweening = false;

    void Start ()
    {
        originalGamePlayPos = GamePlayBtns.anchoredPosition;
        originalSettingsPos = SettingsBtns.anchoredPosition;
    }

    public void ToggleSettings ()
    {
        if (isTweening) return;

        if (IsOpen)
        {
            isTweening = true;
            StartCoroutine(CloseSettings(() =>
            {
                IsOpen = false;
                isTweening = false;
                gameObject.SetActive(false);
                Debug.Log("Settings Menu Closed");
            }));
        }
        else
        {
            isTweening = true;
            gameObject.SetActive(true);
            StartCoroutine(OpenSettings(() =>
            {
                IsOpen = true;
                isTweening = false;
                Debug.Log("Settings Menu Opened");
            }));
        }
    }

    private IEnumerator OpenSettings ( Action OnComplete = null )
    {
        settingsTween = SettingsBtns.DOAnchorPosY(0 , duration).SetEase(Ease.OutBack);
        gamePlayTween = GamePlayBtns.DOAnchorPosY(-200 , duration).SetEase(Ease.InBack);
        betFadeTween = BetAmount.DOFade(0f , duration).SetEase(Ease.OutQuad);

        yield return DOTween.Sequence()
            .Append(gamePlayTween)
            .Append(settingsTween)
            .Join(betFadeTween)
            .WaitForCompletion();

        settingsTween = null;
        gamePlayTween = null;
        OnComplete?.Invoke();
    }

    private IEnumerator CloseSettings ( Action OnComplete = null )
    {
        settingsTween = SettingsBtns.DOAnchorPosY(originalSettingsPos.y , duration).SetEase(Ease.InBack);
        gamePlayTween = GamePlayBtns.DOAnchorPosY(originalGamePlayPos.y , duration).SetEase(Ease.OutBack);
        betFadeTween = BetAmount.DOFade(1f , duration).SetEase(Ease.InQuad);

        yield return DOTween.Sequence()
            .Append(settingsTween)
            .Append(gamePlayTween)
            .Join(betFadeTween)
            .WaitForCompletion();

        settingsTween = null;
        gamePlayTween = null;
        OnComplete?.Invoke();
    }
}
