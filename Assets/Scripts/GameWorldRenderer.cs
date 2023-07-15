using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldRenderer : MonoBehaviour
{
    [SerializeField] private ChunkRenderer chunkPrefab;
    public static Dictionary<Vector2Int, ChunkRenderer> _terrainChunks = new Dictionary<Vector2Int, ChunkRenderer>();
    void Start()
    {
        for(int i = 0; i < GameWorld.LenghtWorldChunks; i++)
        {
            for (int j = 0; j < GameWorld.LenghtWorldChunks; j++)
            {
                _terrainChunks[new Vector2Int(i, j)] = Instantiate(chunkPrefab);
                _terrainChunks[new Vector2Int(i, j)].ChunkCoordinate = i * Vector2Int.right + j * Vector2Int.up;
                _terrainChunks[new Vector2Int(i, j)].ChunkBlocksMaterial = GameWorld._chunksData[new Vector2Int(i, j)];
            }
        }
    }

}
