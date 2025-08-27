using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BoulderConfig
{
    public BoulderSpawner spawner;
    public BoulderPos [] ballPositions;
}
public class Boulders : MonoBehaviour
{
    public BoulderConfig [] boulderConfigs;
    public int maxRepeat = 4;

    private int lastIndex = -1;
    private int repeatCount = 0;

    public int WhichConfig ()
    {
        int configIndex;
        int configCount = boulderConfigs.Length;

        if (lastIndex == -1)
        {
            // First time, pick a random
            configIndex = Random.Range(0 , configCount);
            repeatCount = 1;
        }
        else
        {
            List<int> possibleIndices = new List<int>();

            if (repeatCount >= maxRepeat)
            {
                // Exclude the last index to prevent repeating more than allowed
                for (int i = 0 ; i < configCount ; i++)
                {
                    if (i != lastIndex)
                        possibleIndices.Add(i);
                }
            }
            else
            {
                // All indices are valid
                for (int i = 0 ; i < configCount ; i++)
                {
                    possibleIndices.Add(i);
                }
            }

            configIndex = possibleIndices [Random.Range(0 , possibleIndices.Count)];

            // Update repeat count
            if (configIndex == lastIndex)
                repeatCount++;
            else
                repeatCount = 1;
        }

        lastIndex = configIndex;
        return configIndex;
    }
}
