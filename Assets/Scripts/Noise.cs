using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise
{
    public float[,] GenerateNoiseMap(int mapWidth, int mapHeight)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        for(int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float perlinValue = Mathf.PerlinNoise(x, y);
                noiseMap[x, y] = perlinValue;
            }
        }

        return noiseMap;

    }
}
