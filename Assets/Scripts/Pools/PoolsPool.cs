using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsPool : MonoBehaviour
{
    [SerializeField]
    private BlobsPool blobsPool;

    [SerializeField]
    private EnemiesPool enemiesPool;

    [SerializeField]
    private ProjectilesPool projectilesPool;

    [SerializeField]
    private ParticlesPool enemyParticlesPool;

    private static PoolsPool _instance;
    public static PoolsPool Instance
    {
        get { return _instance; }
    }
    public BlobsPool BlobsPool
    {
        get { return blobsPool; }
    }
    public EnemiesPool EnemiesPool
    {
        get { return enemiesPool; }
    }
    public ProjectilesPool ProjectilesPool
    {
        get { return projectilesPool; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this);
        else
        {
            _instance = this;
        }
    }
}
