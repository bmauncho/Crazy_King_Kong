using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    PoolManager poolManager;
    private void Start ()
    {
        poolManager = CommandCenter.Instance.poolManager_;
    }

    public GameObject spawnBall ()
    {
        GameObject Ball = poolManager.GetFromPool(PoolType.Boulder , transform.position , Quaternion.identity , transform);
        return Ball;
    }
}
