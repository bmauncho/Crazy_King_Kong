using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum TextType
{
    WinText,
    BetText,
    WalletText,
    WinUIText,
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
    public List<(string actual, string available)> charRefrences = new List<(string actual, string available)>
    {
        (".","fullstop"),
        (",", "comma")
    };

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

    public void refreshBetText ( string Input,TMP_Text text )
    {
        TextInfo info = TextInfo(TextType.BetText);
        if(info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                charRefrences
            );
        }
    }

    public void refreshWinText ( string Input , TMP_Text text )
    {
        TextInfo info = TextInfo(TextType.WinText);
        if (info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                charRefrences
            );
        }
    }

    public void refreshWalletText ( string Input , TMP_Text text )
    {
        TextInfo info = TextInfo(TextType.WalletText);
        if (info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                charRefrences
            );
        }
    }

    public void refreshWinUIText ( string Input , TMP_Text text )
    {
        TextInfo info = TextInfo(TextType.WinUIText);
        if (info != null)
        {
            RefreshNumbersText(
                Input ,
                info.prefix ,
                text ,
                info.spriteAsset ,
                charRefrences
            );
        }
    }


    [ContextMenu("Test")]
    public void Test ()
    {
        TextInfo info = TextInfo(TextType.BetText);
        int index = info.spriteAsset.GetSpriteIndexFromName("UI_Bet1_FNT_4");
        Debug.Log("Index test: " + index);

    }
}
