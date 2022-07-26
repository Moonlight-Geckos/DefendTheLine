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
    private ProjectilesPool normalProjectilesPool;

    [SerializeField]
    private ProjectilesPool explosiveProjectilesPool;

    [SerializeField]
    private ParticlesPool explosionParticlesPool;

    [SerializeField]
    private ParticlesPool dropletsPool;

    [SerializeField]
    private ParticlesPool stickmenExplosionPool;

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
    public ProjectilesPool NormalProjectilesPool
    {
        get { return normalProjectilesPool; }
    }
    public ProjectilesPool ExplosiveProjectilesPool
    {
        get { return explosiveProjectilesPool; }
    }
    public ParticlesPool ExplosionParticlesPool
    {
        get { return explosionParticlesPool; }
    }
    public ParticlesPool DropletsPool
    {
        get { return dropletsPool; }
    }
    public ParticlesPool StickmenExplosionPool
    {
        get { return stickmenExplosionPool; }
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
