using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    private GridManager _gridManager;
    private BlobsPool _blobPool;
    private int _spawnCost;
    private void Awake()
    {
        EventsPool.ShouldSpawnBlobEvent.AddListener(SpawnBlob);
        _spawnCost = 1;
    }
    private void Start()
    {
        _blobPool = PoolsPool.Instance.BlobsPool;
        _gridManager = GridManager.Instance;
    }
    private void SpawnBlob()
    {
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
            _spawnCost += 2;
        }
    }
}
