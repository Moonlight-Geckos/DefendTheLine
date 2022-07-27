using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour
{
    public int Level
    {
        get { return _level; }
    }

    private Renderer _renderer;
    private IDisposable _disposable;
    private DataHolder _dataHolder;
    private ProjectilesPool _projectilePool;
    private int _level;

    #region AttackTypes
    Dictionary<int, IEnumerator> _attackTypes;

    IEnumerator lvl1()
    {
        yield return null;
    }
    IEnumerator lvl2()
    {
        yield return null;
    }
    IEnumerator lvl3()
    {
        yield return null;
    }
    IEnumerator lvl4()
    {
        yield return null;
    }


    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.name[0] > name[0])
            Dispose();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.name[0] > name[0])
            Dispose();
    }
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat("_RandomNum", Random.value);

        _projectilePool = PoolsPool.Instance.ProjectilesPool;
        _dataHolder = DataHolder.Instance;

        _attackTypes = new Dictionary<int, IEnumerator>();

        _attackTypes.Add(1, lvl1());
        _attackTypes.Add(2, lvl2());
        _attackTypes.Add(3, lvl3());
        _attackTypes.Add(4, lvl4());
    }
    public void Initialize()
    {
        _level = 0;
        SetupBlob();
    }
    public void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _disposable.Dispose();
    }
    public void Move(Vector3 position)
    {
        IEnumerator move()
        {
            yield return null;
            Vector3 newPos = position * 2;
            newPos.y = 0.5f;

            float duration =  0.45f - 0.012f * (Vector3.Distance(newPos,transform.position) / 2f );
            float elapsed = 0f;
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, newPos, elapsed / duration);
                yield return null;
            }
        }
        StartCoroutine(move());
    }
    public void Merge()
    {
        _level++;
        SetupBlob();
        Enrage();
    }
    public void Enrage()
    {
        IEnumerator enrage()
        {
            float duration = 0.1f;
            float elapsed = 0f;
            yield return new WaitForSeconds(0.22f);
            while (elapsed <= duration)
            {
                elapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.5f, elapsed / duration);
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
    private void SetupBlob()
    {
        int ind = Mathf.Min(_level, _dataHolder.BlobsMaterials.Length - 1);
        _renderer.material = _dataHolder.BlobsMaterials[ind];
        name = _level.ToString();
    }

    private void Attack(Target target)
    {
        IEnumerator atktype;
        if(_attackTypes.TryGetValue(_level, out atktype))
        {
            StartCoroutine(atktype);
        }
    }

}