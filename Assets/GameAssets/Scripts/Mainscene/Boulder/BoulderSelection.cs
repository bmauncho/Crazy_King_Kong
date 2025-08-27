using System.Collections.Generic;
using UnityEngine;

public class BoulderSelection : MonoBehaviour
{
    [Header("boulder type config")]
    public BoulderTypeConfig [] boulderTypeConfig;
    public int maxRepeat = 4;
    private int lastIndex = -1;
    private int repeatCount = 0;
    public BoulderTypeConfig GetRandomBoulderTypeConfig ()
    {
        if (boulderTypeConfig == null || boulderTypeConfig.Length == 0)
        {
            Debug.LogError("BoulderTypeConfig array is null or empty.");
            return null;
        }

        int index = WhichConfig();
        if (index < 0 || index >= boulderTypeConfig.Length)
        {
            Debug.LogError($"Index {index} is out of bounds for BoulderTypeConfig array of length {boulderTypeConfig.Length}.");
            return null;
        }

        return boulderTypeConfig [index];
    }

    public int WhichConfig ()
    {
        int configIndex = 0;
        int configCount = boulderTypeConfig.Length;

        if (configCount <= 0)
        {
            Debug.LogError("No BoulderTypeConfigs available.");
            return 0;
        }

        if (lastIndex == -1)
        {
            configIndex = Random.Range(0 , configCount);
            repeatCount = 1;
        }
        else
        {
            List<int> possibleIndices = new List<int>();

            for (int i = 0 ; i < configCount ; i++)
            {
                // If repeat limit reached, skip the last index
                if (repeatCount >= maxRepeat && i == lastIndex)
                    continue;

                possibleIndices.Add(i);
            }

            if (possibleIndices.Count == 0)
            {
                Debug.LogWarning("Only one boulder type available or all filtered out. Reusing last index.");
                configIndex = lastIndex;
            }
            else
            {
                configIndex = possibleIndices [Random.Range(0 , possibleIndices.Count)];
            }

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
