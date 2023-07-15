using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneChunksTerrainGenerator : MonoBehaviour
{
    
    static public int[,,] GenetateChunksTerrain (int[,,] chunk, Vector2Int chunkCoordinate)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < Mathf.PerlinNoise(((float)x + chunkCoordinate[0] * 16) * 0.02f, ((float)z + chunkCoordinate[1] * 16) * 0.02f) * 60; y++) 
                {
                    chunk[x, y, z] = 1;
                }
            }
        }
        return chunk;
    }

}
