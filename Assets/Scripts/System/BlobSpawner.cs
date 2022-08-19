using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    private BlobsPool _blobPool;
    private int _towerLayerMask;
    private static int _spawnCost;

    public static int SpawnCost
    {
        get { return _spawnCost; }
    }
    private void Awake()
    {
        _towerLayerMask = LayerMask.GetMask("Tower");
        EventsPool.ShouldSpawnBlobEvent.AddListener(SpawnBlob);
        _spawnCost = 0;
    }
    private void Start()
    {
        _blobPool = PoolsPool.Instance.BlobsPool;
    }
    private void SpawnBlob()
    {
        var pos = BlobZone.ZoneBounds.center +
            (Vector3.right * Random.Range(-0.4f, 0.4f) * BlobZone.ZoneBounds.size.x) +
            (Vector3.forward * Random.Range(-0.4f, 0.4f) * BlobZone.ZoneBounds.size.z);
        int counter = 5;
        while(Physics.OverlapSphere(pos, 1, _towerLayerMask).Length > 0 && counter > 0)
        {
            pos = BlobZone.ZoneBounds.center +
                (Vector3.right * Random.Range(-0.4f, 0.4f) * BlobZone.ZoneBounds.size.x) +
                (Vector3.forward * Random.Range(-0.4f, 0.4f) * BlobZone.ZoneBounds.size.z);
            counter--;
        }
        var blob = _blobPool.Pool.Get();
        blob.Initialize();
        blob.transform.position = pos;
    }
}