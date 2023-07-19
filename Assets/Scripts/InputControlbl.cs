using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControlbl : MonoBehaviour
{

    
    private float _xRot;
    private float _yRot;
    private float _xMove;
    private float _zMove;
    private Vector3 _moveDirection;
    private RaycastHit _hit;
    private Ray _ray;
    [SerializeField] Camera _camera;
    [SerializeField] private float Speed = 10f;
    [SerializeField] private float _sensiv = 5f;
    [SerializeField] private CharacterController _character;
    [SerializeField] private ChunkRenderer chunkPrefab;

    private void Start()
    {
        _character = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Movement();
        MouseMove();
        SetupBlock();
        DestroyBlock();
    }

    private void Movement()
    {
        _xMove = Input.GetAxis("Horizontal");
        _zMove = Input.GetAxis("Vertical");
        _moveDirection = transform.TransformDirection(new Vector3(_xMove, 0, _zMove));
        _character.Move(_moveDirection * Time.deltaTime * Speed);

    }
     private void MouseMove()
    {
        _xRot += Input.GetAxis("Mouse X");
        _yRot += Input.GetAxis("Mouse Y");
        _character.transform.rotation = Quaternion.Euler(-_yRot * _sensiv, _xRot * _sensiv, 0);
    }

    private void SetupBlock()
    {
        if(!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if(Physics.Raycast(Camera.main.transform.position, transform.forward, out _hit))
        {
            Vector3 localBlockPos = GetLocalBlockPos(_hit);
            Vector2Int GlobalChunkCoordinate = GetGlobalChunkPos(_hit);
            int[,,] newChunkBlockMaterial = new int[16, 128, 16];
            
            Debug.Log(new Vector3((int)localBlockPos[0], (int)localBlockPos[1], (int)localBlockPos[2]));
            GameWorldRenderer._terrainChunks[GlobalChunkCoordinate].ChunkBlocksMaterial[(int)localBlockPos[0], (int)localBlockPos[1], (int)localBlockPos[2]] = 1;
            newChunkBlockMaterial = GameWorldRenderer._terrainChunks[GlobalChunkCoordinate].ChunkBlocksMaterial;
            GameWorldRenderer._terrainChunks.Remove(GlobalChunkCoordinate);
            GameWorldRenderer._terrainChunks[GlobalChunkCoordinate] = Instantiate(chunkPrefab);
            GameWorldRenderer._terrainChunks[GlobalChunkCoordinate].ChunkCoordinate = GlobalChunkCoordinate;
            GameWorldRenderer._terrainChunks[GlobalChunkCoordinate].ChunkBlocksMaterial = newChunkBlockMaterial;
            Destroy(GameWorldRenderer._terrainChunks[GlobalChunkCoordinate].gameObject);

        }

    }
    private Vector2Int GetGlobalChunkPos(RaycastHit _hit)
    {
        Vector2Int assumedGlobalChunkPos = new Vector2Int((int)(_hit.transform.localPosition / 16)[0], (int)(_hit.transform.localPosition / 16)[2]);
        Vector2Int GlobalChunkPos = assumedGlobalChunkPos;
        Vector3 assumedLocalBlockPos = RoundVector3ToDown(_hit.point - _hit.transform.localPosition + CorrectionOnNormal(_hit.normal));
        if (assumedLocalBlockPos[0] > 15) GlobalChunkPos += new Vector2Int(1, 0);
        if (assumedLocalBlockPos[0] < 0) GlobalChunkPos += new Vector2Int(-1, 0);
        if (assumedLocalBlockPos[2] > 15) GlobalChunkPos += new Vector2Int(0, 1);
        if (assumedLocalBlockPos[2] < 0) GlobalChunkPos += new Vector2Int(0, -1);
        return GlobalChunkPos;
    }
    private Vector3 GetLocalBlockPos(RaycastHit _hit)
    {   
        Vector3 assumedLocalBlockPos = RoundVector3ToDown(_hit.point - _hit.transform.localPosition + CorrectionOnNormal(_hit.normal));
        Vector3 localBlockPos;
        if (assumedLocalBlockPos[0] > 15) assumedLocalBlockPos[0] = 0;
        if (assumedLocalBlockPos[0] < 0) assumedLocalBlockPos[0] = 15;
        if (assumedLocalBlockPos[2] > 15) assumedLocalBlockPos[2] = 0;
        if (assumedLocalBlockPos[2] < 0) assumedLocalBlockPos[2] = 15;
        localBlockPos = assumedLocalBlockPos;
        return localBlockPos;
    }

    private void DestroyBlock()
    {
        if (!Input.GetMouseButtonDown(1))
        {
            return;
        }

        if (Physics.Raycast(Camera.main.transform.position, transform.forward, out _hit))
        {
            Debug.Log("Hit detected");
        }

    }

    private Vector3 RoundVector3ToDown(Vector3 vector)
    {
        return new Vector3((float)Math.Floor(vector[0]), (float)Math.Floor(vector[1]), (float)Math.Floor(vector[2]));
    }

    private Vector3 CorrectionOnNormal(Vector3 normal)
    {
        if (normal[0] + normal[1] + normal[2] < 0) return normal;
        return Vector3.zero;
    }

}
