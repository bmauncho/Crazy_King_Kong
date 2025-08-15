using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum TextType
{
    WinText,
    BetText,
}

[System.Serializable]
public class TextInfo
{
    public TextType textType;
    public string prefix;
    public TMP_SpriteAsset spriteAsset;
    public List<(string actual, string available)> charRefrences;
}

public class TextManager : MonoBehaviour
{
    public TextInfo[] textInfo;
    private void RefreshNumbersText (
        string Input,
        string prefix,
        TMP_Text text,
        TMP_SpriteAsset spriteAsset ,
        List<(string actual, string available)> charRefrences =null)
    {

       TextGenerator.CreateSpriteAssetTextNumbers(
            Input ,
            prefix ,
            text ,
            spriteAsset ,
            charRefrences
       );
    }

    public TextInfo TextInfo ( TextType textType )
    {
        foreach (var info in textInfo)
        {
            if (info.textType == textType)
            {
                return info;
            }
        }
        return null;
    }

    public void refreshBetText ( string Input,TMP_Text text , List<(string actual, string available)> charRefrences = null )
    {
        TextInfo info = TextInfo(TextType.BetText);
        if(info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                info.charRefrences
            );
        }
    }

    public void refreshWinText ( string Input , TMP_Text text , List<(string actual, string available)> charRefrences = null )
    {
        TextInfo info = TextInfo(TextType.WinText);
        if (info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                info.charRefrences
            );
        }
    }
}
