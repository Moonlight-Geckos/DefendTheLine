using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    private BlobsPool _blobPool;
    private static int _spawnCost;

    public static int SpawnCost
    {
        get { return _spawnCost; }
    }
    private void Awake()
    {
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

        var blob = _blobPool.Pool.Get();
        blob.Initialize();
        blob.transform.position = pos;
        /*
        if(Observer.AvailablePoints < _spawnCost)
            return;
        var pos = _gridManager.GetEmptyCell();
        if (pos != null)
        { 
            var blob = _blobPool.Pool.Get();
            blob.transform.position = new Vector3(pos.Item2 * 2, 0.5f, pos.Item1 * 2);
            blob.Initialize(new Vector3(pos.Item2, 0, pos.Item1));
            _gridManager.AddBlob(blob, pos);
            Observer.AvailablePoints -= _spawnCost;
            _spawnCost ++;
        }
        */
    }
}