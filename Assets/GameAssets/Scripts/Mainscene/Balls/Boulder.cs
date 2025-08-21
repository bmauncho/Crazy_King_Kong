using UnityEngine;

public enum BoulderType
{
    Rock,
    Lava,
    Ice,
    Gold,
}
public class Boulder : MonoBehaviour
{
    public BoulderType boulderType;
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
}
