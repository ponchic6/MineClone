using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldRenderer : MonoBehaviour
{
    private int LenghtWorldChunks = 4;
    [SerializeField] private ChunkRenderer chunkPrefab;
    public static Dictionary<Vector2Int, ChunkRenderer> _terrainChunks = new Dictionary<Vector2Int, ChunkRenderer>();
    void Start()
    {
        for(int i = 0; i < LenghtWorldChunks; i++)
        {
            for (int j = 0; j < LenghtWorldChunks; j++)
            {
                _terrainChunks[new Vector2Int(i, j)] = Instantiate(chunkPrefab);
                _terrainChunks[new Vector2Int(i, j)].ChunkCoordinate = i * Vector2Int.right + j * Vector2Int.up;
                _terrainChunks[new Vector2Int(i, j)].ChunkBlocksMaterial = OneChunksTerrainGenerator.GenetateChunksTerrain(new int[16, 128, 16], new Vector2Int(i, j));
            }
        }
    }

}
