using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    static public int LenghtWorldChunks = 4;
    static public Dictionary<Vector2Int, int[,,]> _chunksData = new Dictionary<Vector2Int, int[,,]>();

    void Awake()
    {   
        for(int i = 0; i < LenghtWorldChunks; i++)
        {
            for(int j = 0; j < LenghtWorldChunks; j++)
            {
                _chunksData.Add(new Vector2Int(i, j), new int[16, 128, 16]);
                _chunksData[new Vector2Int(i, j)] = OneChunksTerrainGenerator.GenetateChunksTerrain(_chunksData[new Vector2Int(i, j)], new Vector2Int(i, j));
            }
        }


    }

}
