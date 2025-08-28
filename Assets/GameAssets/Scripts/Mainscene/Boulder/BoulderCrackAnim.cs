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
        if (crack != null)
        {
            crack.sprite = cracks [crackLevel];
            if (crackLevel <= cracks.Length-1)
            {
                crackLevel++;
            }
        }
    }

    public void ResetCrack ()
    {
        crackLevel = 0;
        crack.gameObject.SetActive(false);
    }
}
