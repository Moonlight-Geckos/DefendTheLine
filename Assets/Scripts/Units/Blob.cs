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
    private ProjectilesPool _projectilePool;

    private Target _currentTarget;
    private float _shootingCooldown;
    private Vector3 _currentPos;
    private bool _moving;

    #region AttackTypes
    Dictionary<int, string> _attackTypes;

    IEnumerator lvl0()
    {
        Shoot(_currentTarget);
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
            Shoot(closetar);
            cnt++;
        }
        Debug.Log(cnt);
        yield return null;

    }
    IEnumerator lvl2()
    {
        for (int i = 0; i < 3; i++)
        {
            if(_currentTarget != null)
                Shoot(_currentTarget);
            yield return new WaitForSeconds(0.12f);
        }
    }
    IEnumerator lvl3()
    {
        Shoot(_currentTarget);
        yield return null;
    }
    IEnumerator lvl4()
    {
        Shoot(_currentTarget);
        _shootingCooldown -= 0.5f;
        yield return null;
    }

    #endregion

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
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat("_RandomNum", Random.value);

        _projectilePool = PoolsPool.Instance.ProjectilesPool;
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
    public void Initialize()
    {
        _level = 0;
        _currentTarget = null;
        _shootingCooldown = 0;
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
        _currentPos = position * 2;
        IEnumerator move()
        {
            _moving = true;
            _currentPos.y = 0.5f;

            float duration =  0.45f - 0.012f * (Vector3.Distance(_currentPos, transform.position) / 2f );
            float elapsed = 0f;
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, _currentPos, elapsed / duration);
                yield return null;
            }
            _moving = false;
        }
        StopCoroutine("move");
        StartCoroutine(move());
    }
    public void Merge()
    {
        _level++;
        SetupColorAndName();
    }
    private void Wobble(float duration, Vector3 direction)
    {
        IEnumerator wobble()
        {
            float elapsed = 0f;
            float d;
            direction = direction.normalized * 0.2f;
            direction.y = 0;
            Vector3 a = _currentPos + direction;
            while(_moving)
                yield return null;
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                d = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.LerpUnclamped(_currentPos, a, (1-d) * Mathf.Sin(Mathf.PI * d * 8));
                yield return null;
            }
        }
        Enrage(duration / 3f, 1.55f);
        StartCoroutine(wobble());
    }
    private void Enrage(float duration, float scaleUp)
    {
        IEnumerator enrage()
        {
            float elapsed = 0f;
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scaleUp, elapsed / duration);
                yield return null;
            }
            elapsed = 0f;
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, elapsed / duration);
                yield return null;
            }
        }
        StartCoroutine(enrage());
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
    private void Shoot(Target target)
    {
        Enrage(0.1f, 1.35f);
        var projectile = _projectilePool.Pool.Get();
        projectile.transform.position = transform.position;
        projectile.Initialize(_renderer.material, target, projectileSpeed);
        _shootingCooldown = (startingShootingCooldown - decreasingShootingCooldownPerLevel * _level) + Random.Range(0.1f, 0.23f);
    }

}