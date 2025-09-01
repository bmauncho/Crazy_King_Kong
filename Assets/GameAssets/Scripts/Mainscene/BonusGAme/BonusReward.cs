using UnityEngine;
using UnityEngine.UI;

public class BonusReward : MonoBehaviour
{
    public Image Bg;
    public Image Icon;
    public BonusOptions Option;

    public void SetData (Sprite Bg_,Sprite Icon_,BonusOptions option)
    {
        Bg.sprite = Bg_;
        Icon.sprite = Icon_;
        Option = option;
    }
}
