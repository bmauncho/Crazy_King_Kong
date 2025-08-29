using TMPro;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    public TMP_Text winAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWinUI ()
    {
        gameObject.SetActive(true);
    }

    public void HideWinUI ()
    {
        gameObject.SetActive(false);
    }
}
