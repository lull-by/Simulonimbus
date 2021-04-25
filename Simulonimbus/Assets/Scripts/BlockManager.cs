using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 SpawnBoxPoint1 = new Vector3(-6, 6, 6);

    [SerializeField]
    private Vector3 SpawnBoxPoint2 = new Vector3(-7, -6, -6);

    [SerializeField, Min(0)]
    private float SpawnsPerSecond = 10;

    [SerializeField]
    private GameObject BlockPrefab;

    [SerializeField]
    private float BlockDestroyX = 5; // X value that blocks are to be destroyed at

    private float maxX, minX, maxY, minY, maxZ, minZ;
    private bool isBlockPrefabNotNull;
    private float secondsPerSpawn;

    void Start()
    {
        isBlockPrefabNotNull = BlockPrefab != null;
        secondsPerSpawn = 1 / SpawnsPerSecond;
        CalculateMinMaxSpawnVectors();
        StartCoroutine(SpawnBlocks());
    }

    IEnumerator SpawnBlocks()
    {
        while (true)
        {
            SpawnOneBlock();
            yield return new WaitForSecondsRealtime(secondsPerSpawn);
        }
    }

    void SpawnOneBlock()
    {
        var spawnPosition = GetPositionInSpawnBox();
        if (isBlockPrefabNotNull)
        {
            var blockGO = Instantiate(BlockPrefab, spawnPosition, Quaternion.identity);
            var block = blockGO.GetComponent<Block>();
            block.maxX = BlockDestroyX;
        }
    }
    
    void CalculateMinMaxSpawnVectors()
    {
        var p1 = SpawnBoxPoint1;
        var p2 = SpawnBoxPoint2;
        maxX = Mathf.Max(p1.x, p2.x);
        minX = Mathf.Min(p1.x, p2.x);
        maxY = Mathf.Max(p1.y, p2.y);
        minY = Mathf.Min(p1.y, p2.y);
        maxZ = Mathf.Max(p1.z, p2.z);
        minZ = Mathf.Min(p1.z, p2.z);
    }

    Vector3 GetPositionInSpawnBox()
    {
        var p1 = SpawnBoxPoint1;
        var p2 = SpawnBoxPoint2;
        return new Vector3(
            x: Random.Range(minX, maxX),
            y: Random.Range(minY, maxY),
            z: Random.Range(minZ, maxZ)
        );
    }
}
