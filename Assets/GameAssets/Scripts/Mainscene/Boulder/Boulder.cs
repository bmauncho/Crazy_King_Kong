using UnityEngine;
using UnityEngine.UI;

public class Boulder : MonoBehaviour
{
    public BoulderType boulderType;
    public Image boulderImg;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBoulderType ( BoulderType type )
    {
        boulderType = type;
    }

    public void SetBoulderSprite ( Sprite sprite )
    {
        boulderImg.sprite = sprite;
    }
}
