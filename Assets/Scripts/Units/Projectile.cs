using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Renderer _renderer;
    private Target _target;
    private float _velocity;
    private Vector3 _direction;
    private IDisposable _disposable;

    private bool _canDamage;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (_target == null)
            return;
        if (!_target.gameObject.activeSelf)
            _canDamage = false;

        transform.position += _direction * _velocity * Time.deltaTime;
        if(transform.position.z >= _target.transform.position.z)
        {

            if(_canDamage)
                _target.GetDamage(1);
            Dispose();
        }
    }
    private void Dispose()
    {
        if (_disposable == null)
            _disposable = GetComponent<IDisposable>();
        _target = null;
        _disposable.Dispose();
    }
    public void Initialize(Material mat, Target target, float velocity)
    {
        _target = target;

        _direction = (_target.transform.position - transform.position).normalized;

        _renderer.material = mat;
        _velocity = velocity;
        _canDamage = true;
    }
}
