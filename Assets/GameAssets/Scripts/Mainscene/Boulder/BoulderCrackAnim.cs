using UnityEngine;
using UnityEngine.UI;

public class BoulderCrackAnim : MonoBehaviour
{
    public int crackLevel = 0;
    public Image crack;
    public Sprite [] cracks;

    public void ShowCrack ()
    {
        crack.gameObject.SetActive(true);
        var sr = crack.GetComponent<Image>();
        if (sr != null)
        {
            sr.sprite = cracks [crackLevel];
            crackLevel++;
        }
    }

    public void ResetCrack ()
    {
        crackLevel = 0;
        crack.gameObject.SetActive(false);
    }
}
