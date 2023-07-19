using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class ChunkRenderer : MonoBehaviour
{
    public const int ChunkWidth = 16;
    public const int ChunkHeight = 128;
    private List<Vector3> _verticies = new List<Vector3>();
    private List<int> _triangles = new List<int>();

    [HideInInspector] public Vector2Int ChunkCoordinate;
    public int[,,] ChunkBlocksMaterial = new int[ChunkWidth, ChunkHeight, ChunkWidth];

    void Start()
    {
        Mesh chunkMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = chunkMesh;
        gameObject.transform.position = new Vector3(ChunkCoordinate[0] * 16, 0, ChunkCoordinate[1] * 16);

        for (int y = 0; y < ChunkHeight; y++) 
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++) 
                {
                    if (ChunkBlocksMaterial[x, y, z] == 1)
                    {
                        CubeCreator(x, y, z);
                    }
                }
            }
        }

        chunkMesh.vertices = _verticies.ToArray();
        chunkMesh.triangles = _triangles.ToArray();

        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    private void CubeCreator(int x, int y, int z)
    {
        if (GetBlockPosition(x, y, z - 1) == 0) AntiZWallCreator(x, y, z);
        if (GetBlockPosition(x, y, z + 1) == 0) ZWallCreator(x, y, z);
        if (GetBlockPosition(x + 1, y, z) == 0) XWallCreator(x, y, z);
        if (GetBlockPosition(x - 1, y, z) == 0) AntiXWallCreator(x, y, z);
        if (GetBlockPosition(x, y + 1, z) == 0) YWallCreator(x, y, z);
        if (GetBlockPosition(x, y - 1, z) == 0) AntiYwallCreator(x, y, z);
    }

    private int GetBlockPosition(int x, int y, int z)
    {
        if (x >= 0 && x < ChunkWidth &&
            z >= 0 && z < ChunkWidth &&
            y >= 0 && y < ChunkHeight)
        {
            return ChunkBlocksMaterial[x, y, z];
        }
        else if (x < 0 && GameWorldRenderer._terrainChunks.ContainsKey(ChunkCoordinate + Vector2Int.left))
            return GameWorldRenderer._terrainChunks[ChunkCoordinate + Vector2Int.left].ChunkBlocksMaterial[15, y, z];
        else if (x > 15 && GameWorldRenderer._terrainChunks.ContainsKey(ChunkCoordinate + Vector2Int.right))
            return GameWorldRenderer._terrainChunks[ChunkCoordinate + Vector2Int.right].ChunkBlocksMaterial[0, y, z];
        else if (z < 0 && GameWorldRenderer._terrainChunks.ContainsKey(ChunkCoordinate + Vector2Int.down))
            return GameWorldRenderer._terrainChunks[ChunkCoordinate + Vector2Int.down].ChunkBlocksMaterial[x, y, 15];
        else if (z > 15 && GameWorldRenderer._terrainChunks.ContainsKey(ChunkCoordinate + Vector2Int.up))
            return GameWorldRenderer._terrainChunks[ChunkCoordinate + Vector2Int.up].ChunkBlocksMaterial[x, y, 0];
        else return 0;
    }


    private void AntiZWallCreator(int x, int y, int z)
    {
        _verticies.Add(new Vector3(0 + x, 0 + y, 0 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 0 + z));
        _verticies.Add(new Vector3(1 + x, 1 + y, 0 + z));
        _verticies.Add(new Vector3(0 + x, 1 + y, 0 + z));

        TriangleAdder();
    }

    private void ZWallCreator(int x, int y, int z)
    {

        _verticies.Add(new Vector3(0 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(1 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 1 + z));
        _verticies.Add(new Vector3(0 + x, 0 + y, 1 + z));

        TriangleAdder();
    }

    private void XWallCreator(int x, int y, int z)
    {
        _verticies.Add(new Vector3(1 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(1 + x, 1 + y, 0 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 0 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 1 + z));

        TriangleAdder();
    }

    private void AntiXWallCreator(int x, int y, int z)
    {
        _verticies.Add(new Vector3(0 + x, 1 + y, 0 + z));
        _verticies.Add(new Vector3(0 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(0 + x, 0 + y, 1 + z));
        _verticies.Add(new Vector3(0 + x, 0 + y, 0 + z));

        TriangleAdder();
    }

    private void YWallCreator(int x, int y, int z)
    {
        _verticies.Add(new Vector3(1 + x, 1 + y, 0 + z));
        _verticies.Add(new Vector3(1 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(0 + x, 1 + y, 1 + z));
        _verticies.Add(new Vector3(0 + x, 1 + y, 0 + z));
        
        TriangleAdder();
        
    }

    private void AntiYwallCreator(int x, int y, int z)
    {
        _verticies.Add(new Vector3(0 + x, 0 + y, 0 + z));
        _verticies.Add(new Vector3(0 + x, 0 + y, 1 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 1 + z));
        _verticies.Add(new Vector3(1 + x, 0 + y, 0 + z));

        TriangleAdder();
    }

    private void TriangleAdder()
    {
        _triangles.Add(_verticies.Count + 0 - 4);
        _triangles.Add(_verticies.Count + 3 - 4);
        _triangles.Add(_verticies.Count + 1 - 4);
        _triangles.Add(_verticies.Count + 3 - 4);
        _triangles.Add(_verticies.Count + 2 - 4);
        _triangles.Add(_verticies.Count + 1 - 4);
    }

}
