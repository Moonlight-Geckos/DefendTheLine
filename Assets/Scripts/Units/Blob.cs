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
    private Observer _observer;

    private Target _currentTarget;
    private ProjectilesPool _normalProjectilesPool;
    private ProjectilesPool _explosiveProjectilesPool;

    private Renderer _renderer;
    private Collider _collider;
    private Rigidbody _rb;
    private BlobAnimator _blobAnimator;
    private IDisposable _disposable;

    private Color _mainColor;

    private Vector3 _lastVelocity;
    private float _shootingCooldown;
    private string _atktype;
    private bool _levelingUp;
    private GameObject _lastMergedBlob;

    public Color MainColor
    {
        get { return _mainColor; }
    }

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
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponentInChildren<Renderer>();
        _renderer.material.SetFloat("_RandomNum", Random.value);

        _blobAnimator = GetComponentInChildren<BlobAnimator>();

        _normalProjectilesPool = PoolsPool.Instance.NormalProjectilesPool;
        _explosiveProjectilesPool = PoolsPool.Instance.ExplosiveProjectilesPool;
        _observer = Observer.Instance;

        _attackTypes = new Dictionary<int, string>();
        _attackTypes.Add(0, "lvl0");
        _attackTypes.Add(1, "lvl1");
        _attackTypes.Add(2, "lvl2");
        _attackTypes.Add(3, "lvl3");
    }
    private void Update()
    {
        if (_level == -1)
            return;
        if (_shootingCooldown > 0)
            _shootingCooldown -= Time.deltaTime;

        _currentTarget = _observer.GetClosestTarget(transform.position);
        if (_currentTarget == null || !_currentTarget.gameObject.activeSelf)
        {
            return;
        }
    }
    private void LateUpdate()
    {
        _lastMergedBlob = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Merge
        if (collision.collider.name[0] == name[0] && _level > -1)
        {
            var otherBlob = collision.collider.GetComponent<Blob>();
            otherBlob.LevelUp(this.gameObject);
            MergeIntoAnother(collision.collider.transform);
            _level = -1;
        }
        // Reflect
        else
        {
            if ((collision.collider.name[0] == 'B' || collision.collider.gameObject != _lastMergedBlob)
                && Vector3.Dot(_lastVelocity, collision.contacts[0].normal) < 0)
            {
                _blobAnimator.Squash(collision.contacts[0].normal);

                _rb.velocity = Vector3.Reflect(_lastVelocity, collision.contacts[0].normal);
            }
            else
            {
                _rb.velocity = _lastVelocity;
            }
            _lastVelocity = _rb.velocity;
        }
    }
    public void Initialize()
    {
        _level = 0;
        _levelingUp = false;
        _currentTarget = null;
        _shootingCooldown = 0;
        _collider.enabled = true;
        transform.localScale = Vector3.one;
        _rb.velocity = Vector3.forward * Random.Range(-10, 10f) + Vector3.right * Random.Range(-10, 10f);
        _lastVelocity = _rb.velocity;
        SetupVisuals();
    }
    public void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _disposable.Dispose();
    }
    public void Move(Vector3 position)
    {
        /*
        Vector3 _originalPos = _currentPos;
        _currentPos = position * 2;
        _currentPos.y = 0.5f;

        IEnumerator move(float cellSpeed)
        {
            float elapsed = 0f;
            _moving = true;
            while (elapsed <= 1)
            {
                elapsed += Time.deltaTime * cellSpeed;
                transform.position = Vector3.Lerp(_originalPos, _currentPos, Mathf.Clamp01(elapsed));
                yield return null;
            }
            _moving = false;
        }
        StartCoroutine(move(6f));
        */
    }
    private void MergeIntoAnother(Transform parentBlob)
    {
        var parent = transform.parent;
        _collider.enabled = false;
        _rb.velocity = Vector3.zero;
        transform.parent = parentBlob;

        IEnumerator animate()
        {
            float elapsed = 0;

            yield return null;
            var ors = transform.localScale;
            var orp = transform.localPosition;
            while (elapsed < 1)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(ors, Vector3.zero, Mathf.Clamp01(elapsed));
                transform.localPosition = Vector3.Lerp(orp, Vector3.zero, Mathf.Clamp01(elapsed));
                yield return null;
            }
            transform.parent = parent;
            Dispose();
        }
        StartCoroutine(animate());
        _level = -1;
    }
    public void LevelUp(GameObject fromBlob)
    {
        _level++;
        _lastMergedBlob = fromBlob;
        SetupVisuals(true);
        IEnumerator scaleUp()
        {
            float elapsed = 0;
            var or = transform.localScale;
            var s = Vector3.one + (Vector3.one * _level * 0.2f);
            _levelingUp = true;
            while (elapsed < 1)
            {
                elapsed += Time.deltaTime;
                elapsed = Mathf.Clamp01(elapsed);
                transform.localScale = Vector3.Lerp(or, s, elapsed);
                yield return null;
            }
            _levelingUp = false;
        }
        StartCoroutine(scaleUp());
    }
    private void SetupVisuals(bool lerpColors = false)
    {
        name = _level.ToString();
        var mat = ThemeManager.Instance.GetBlobMaterialByLevel(_level);
        if (!lerpColors)
        {
            _renderer.material = mat;
        }
        else
        {
            IEnumerator lerp()
            {
                float elapsed = 0;
                var b = _renderer.material.color;
                var f = _renderer.material.GetColor("_FresnelColor");
                var b2 = mat.color;
                var f2 = mat.GetColor("_FresnelColor");
                while (elapsed < 1)
                {
                    elapsed += Time.deltaTime;
                    elapsed = Mathf.Clamp01(elapsed);
                    _renderer.material.color = Color.Lerp(b, b2, elapsed);
                    _renderer.material.SetColor("_FresnelColor", Color.Lerp(f, f2, elapsed));
                    yield return null;
                }
            }
            StartCoroutine(lerp());
        }
    }

    private void Attack()
    {
        _attackTypes.TryGetValue(_level % 5, out _atktype);
        if (_atktype != null)
        {
            if (gameObject.activeSelf && !_levelingUp)
            {
                StartCoroutine(_atktype);
            }
        }
        else
        {
            if (gameObject.activeSelf && !_levelingUp)
            {
                StartCoroutine("lvl0");
            }
        }
    }
    private void Shoot(ProjectilesPool pool, Target target)
    {
        var projectile = pool.Pool.Get();
        projectile.transform.position = transform.position;
        projectile.Initialize(_renderer.material, target, projectileSpeed, _level + 1);
        _shootingCooldown = (startingShootingCooldown - decreasingShootingCooldownPerLevel * _level) + Random.Range(0.1f, 0.23f);
    }
}