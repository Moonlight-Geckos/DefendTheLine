using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{

    [SerializeField]
    private float projectileSpeed = 60f;

    [SerializeField]
    private float startingShootingCooldown = 1f;

    [SerializeField]
    private float decreasingShootingCooldownPerLevel = 0.05f;
    public int Level
    {
        get { return _level; }
    }
    private int _level;
    private DataHolder _dataHolder;
    private Observer _observer;
    private IDisposable _disposable;

    private Renderer _renderer;
    private ProjectilesPool _normalProjectilesPool;
    private ProjectilesPool _explosiveProjectilesPool;

    private Target _currentTarget;
    private float _shootingCooldown;
    public Vector3 _currentPos;
    private bool _moving;
    private BlobAnimator _blobAnimator;

    #region AttackTypes
    Dictionary<int, string> _attackTypes;

    IEnumerator lvl0()
    {
        Shoot(_normalProjectilesPool, _currentTarget);
        yield return null;
    }
    IEnumerator lvl1()
    {
        int cnt = 0;
        var closetars = _observer.GetCloseTargets(transform.position);
        foreach(var closetar in closetars)
        {
            if (cnt == 2)
                break;
            Shoot(_normalProjectilesPool, closetar);
            cnt++;
        }
        yield return null;

    }
    IEnumerator lvl2()
    {
        for (int i = 0; i < 3; i++)
        {
            if(_currentTarget != null)
                Shoot(_normalProjectilesPool, _currentTarget);
            yield return new WaitForSeconds(0.12f);
        }
    }
    IEnumerator lvl3()
    {
        Shoot(_explosiveProjectilesPool, _currentTarget);
        yield return null;
    }
    IEnumerator lvl4()
    {
        Shoot(_normalProjectilesPool, _currentTarget);
        _shootingCooldown -= 0.5f;
        yield return null;
    }

    #endregion
    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.material.SetFloat("_RandomNum", Random.value);

        _blobAnimator = GetComponentInChildren<BlobAnimator>();

        _normalProjectilesPool = PoolsPool.Instance.NormalProjectilesPool;
        _explosiveProjectilesPool = PoolsPool.Instance.ExplosiveProjectilesPool;
        _dataHolder = DataHolder.Instance;
        _observer = Observer.Instance;

        _attackTypes = new Dictionary<int, string>();
        _attackTypes.Add(0, "lvl0");
        _attackTypes.Add(1, "lvl1");
        _attackTypes.Add(2, "lvl2");
        _attackTypes.Add(3, "lvl3");
    }
    private void Update()
    {
        if (_shootingCooldown > 0)
            _shootingCooldown -= Time.deltaTime;
        if(_currentTarget == null || !_currentTarget.gameObject.activeSelf)
        {
            _currentTarget = _observer.GetClosestTarget(transform.position);
        }
        else if(_shootingCooldown <= 0)
        {
            Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name[0] > name[0] && _level > -1)
        {
            other.GetComponent<Blob>().Wobble(0.45f, (other.transform.position - transform.position).normalized);
            _level = -1;
            Dispose();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name[0] > name[0] && _level > -1)
        {
            other.GetComponent<Blob>().Wobble(0.45f, (other.transform.position - transform.position).normalized);
            _level = -1;
            Dispose();
        }
    }
    public void Initialize(Vector3 pos)
    {
        _level = 0;
        _currentTarget = null;
        _shootingCooldown = 0;
        _currentPos = pos * 2f;
        _currentPos.y = 0.5f;
        SetupColorAndName();
    }
    public void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _disposable.Dispose();
    }
    public void Move(Vector3 position)
    {
        Vector3 _originalPos = _currentPos;
        _currentPos = position * 2;
        _currentPos.y = 0.5f;

        IEnumerator move(float cellSpeed)
        {
            float elapsed = 0f;
            while (elapsed <= 1)
            {
                elapsed += Time.deltaTime * cellSpeed;
                transform.position = Vector3.Lerp(_originalPos, _currentPos, Mathf.Clamp01(elapsed));
                yield return null;
            }
        }
        StartCoroutine(move(6f));
    }
    public void Merge()
    {
        _level++;
        SetupColorAndName();
    }
    private void Wobble(float duration, Vector3 direction)
    {
        Enrage(duration / 3f, 1.55f);
        _blobAnimator.Wobble(duration, direction);
    }
    private void Enrage(float duration, float scaleUp)
    {
        _blobAnimator.Enrage(duration, scaleUp);
    }
    private void SetupColorAndName()
    {
        int ind = Mathf.Min(_level, _dataHolder.BlobsMaterials.Length - 1);
        _renderer.material = _dataHolder.BlobsMaterials[ind];
        name = _level.ToString();
    }

    private void Attack()
    {
        string atktype;

        if (_attackTypes.TryGetValue(_level, out atktype))
        {
            if(gameObject.activeSelf)
                StartCoroutine(atktype);
        }
    }
    private void Shoot(ProjectilesPool pool, Target target)
    {
        Enrage(0.1f, 1.35f);
        var projectile = pool.Pool.Get();
        projectile.transform.position = transform.position;
        projectile.Initialize(_renderer.material, target, projectileSpeed);
        _shootingCooldown = (startingShootingCooldown - decreasingShootingCooldownPerLevel * _level) + Random.Range(0.1f, 0.23f);
    }

}