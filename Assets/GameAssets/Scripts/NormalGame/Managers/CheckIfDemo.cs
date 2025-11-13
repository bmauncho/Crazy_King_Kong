using UnityEngine;

public class CheckIfDemo : MonoBehaviour
{
    public GameObject demoObjects;

    // Update is called once per frame
    void Update()
    {
        if (CommandCenter.Instance)
        {
            if(CommandCenter.Instance.gameMode == GameMode.Demo)
            {
                demoObjects.SetActive(true);
            }
            else
            {
               demoObjects.SetActive(false);
            }
        }
    }
}
