using UnityEngine;
using UnityEngine.UI;

public class BoulderCrackAnim : MonoBehaviour
{
    public int crackLevel = 0;
    public Image crack;
    public Sprite [] cracks;

    public void ShowCrack ()
    {
        if (crack == null || cracks == null || cracks.Length == 0)
            return;

        crack.gameObject.SetActive(true);

        // Clamp crackLevel to valid range
        crackLevel = Mathf.Clamp(crackLevel , 0 , cracks.Length - 1);
        crack.sprite = cracks [crackLevel];

        // Increment only if not at the last crack level
        if (crackLevel < cracks.Length - 1)
        {
            crackLevel++;
        }
    }

    public void ResetCrack ()
    {
        crackLevel = 0;
        if (crack != null)
        {
            crack.gameObject.SetActive(false);
        }
    }
}
