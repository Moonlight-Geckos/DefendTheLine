using UnityEngine;

public class BlobSpawner : MonoBehaviour
{
    private GridManager _gridManager;
    private BlobsPool _blobPool;
    private void Awake()
    {
        EventsPool.ShouldSpawnBlobEvent.AddListener(SpawnBlob);
    }
    private void Start()
    {
        _blobPool = PoolsPool.Instance.BlobsPool;
        _gridManager = GridManager.Instance;
    }
    private void SpawnBlob()
    {
        var pos = _gridManager.GetEmptyCell();
        if (pos != null)
        { 
            var blob = _blobPool.Pool.Get();
            blob.transform.position = new Vector3(pos.Item2 * 2, 0.5f, pos.Item1 * 2);
            _gridManager.AddBlob(blob, pos);
        }
    }
}
