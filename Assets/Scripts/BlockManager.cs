using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockManager : MonoBehaviour
{
    public Vector3 spawnBoxPoint1 = new Vector3(-6, 6, 6);
    public Vector3 spawnBoxPoint2 = new Vector3(-7, -6, -6);
    public int spawnsPerSecond = 10;
    public GameObject blockPrefab;
    public float destroyX = 5; // X value that blocks are to be destroyed at
    private float _maxX, _minX, _maxY, _minY, _maxZ, _minZ;
    private bool _isBlockPrefabNotNull;
    private float _secondsPerSpawn;

    void Start()
    {
        _isBlockPrefabNotNull = blockPrefab != null;
        _secondsPerSpawn = 1 / spawnsPerSecond;
        CalculateMinMaxSpawnVectors();
        StartCoroutine(SpawnBlocks());
    }

    IEnumerator SpawnBlocks()
    {
        while (true)
        {
            SpawnOneBlock();
            yield return new WaitForSeconds(_secondsPerSpawn);
        }
    }

    void SpawnOneBlock()
    {
        var spawnPosition = GetPositionInSpawnBox();
        if (_isBlockPrefabNotNull)
        {
            var blockGO = Instantiate(blockPrefab, spawnPosition, Quaternion.identity);
            var block = blockGO.GetComponent<Block>();
            block.maxX = destroyX;
        }
    }
    
    #region Helper Methods
    void CalculateMinMaxSpawnVectors()
    {
        var p1 = spawnBoxPoint1;
        var p2 = spawnBoxPoint2;
        _maxX = Mathf.Max(p1.x, p2.x);
        _minX = Mathf.Min(p1.x, p2.x);
        _maxY = Mathf.Max(p1.y, p2.y);
        _minY = Mathf.Min(p1.y, p2.y);
        _maxZ = Mathf.Max(p1.z, p2.z);
        _minZ = Mathf.Min(p1.z, p2.z);
    }

    Vector3 GetPositionInSpawnBox()
    {
        var p1 = spawnBoxPoint1;
        var p2 = spawnBoxPoint2;
        return new Vector3(
            x: Random.Range(_minX, _maxX),
            y: Random.Range(_minY, _maxY),
            z: Random.Range(_minZ, _maxZ)
        );
    }
    #endregion
}
