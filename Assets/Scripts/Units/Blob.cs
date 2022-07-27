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

        Initialize();
    }
    public void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _disposable.Dispose();
    }
    public void Move(SwipeDirection direction)
    {
        IEnumerator move()
        {
            yield return null;
            Vector3 newPos = transform.position;
            Vector3 oldPos = transform.position;
            if (direction == SwipeDirection.Down)
                newPos.z -= 2;

            else if (direction == SwipeDirection.Up)
                newPos.z += 2;

            else if(direction == SwipeDirection.Left)
                newPos.x -= 2;

            else
                newPos.x += 2;

            float duration = 0.2f;
            float elapsed = 0f;
            float percent;
            while (elapsed <= duration)
            {
                elapsed = elapsed + Time.deltaTime;
                percent = Mathf.Clamp01(elapsed / duration);

                transform.position = Vector3.Lerp(oldPos, newPos, percent);
                yield return null;
            }
        }
        StartCoroutine(move());
    }
    public void Merge()
    {
        _level++;
        Initialize();
    }
    private void Initialize()
    {
        int ind = Mathf.Min(_level, _dataHolder.BlobsMaterials.Length);
        _renderer.material = _dataHolder.BlobsMaterials[ind];
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