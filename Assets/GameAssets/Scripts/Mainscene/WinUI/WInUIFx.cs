using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class WinUIFx : MonoBehaviour
{
    public TMP_Text winUIText;

    private PayOutManager payOutMan_;
    private TextManager textMan_;
    private PoolManager poolMan_;

    private RectTransform rect;

    public void ShowWinFx (Action OnComplete = null)
    {
        rect = GetComponent<RectTransform>();
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        payOutMan_ = CommandCenter.Instance.payOutManager_;
        textMan_ = CommandCenter.Instance.textManager_;
        poolMan_ = CommandCenter.Instance.poolManager_;

        string payOut = payOutMan_.GetWinAmount().ToString();
        payOut = NumberFormatter.FormatString(payOut, 2, true);
        textMan_.refreshWinUIText(payOut , winUIText);

        // Reset position and scale if reusing from pool
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;

        Sequence winSequence = DOTween.Sequence();
        winSequence.Append(rect.DOScale(1.2f , 0.5f))
                   .Append(rect.DOAnchorPosY(90f , 0.5f))
                   .Join(rect.DOScale(1f , 0.5f))
                   .Join(canvasGroup.DOFade(0,.5f))
                   .AppendInterval(.5f)
                   .AppendCallback(() =>
                   {
                       OnComplete?.Invoke();
                   });
    }
}
