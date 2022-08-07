using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Target
{
    [SerializeField]
    private float velocity;

    [SerializeField]
    private Color hitColor = Color.white;

    private Rigidbody _rb;
    private ParticlesPool _particlesPool;
    private Dictionary<Material, Color> _materials;
    private Renderer _renderer;
    private Transform _riggedTransform;
    private float _originalScale;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();
        _particlesPool = PoolsPool.Instance.StickmenExplosionPool;
        _riggedTransform = transform.Find("RiggedMesh");
        _originalScale = _riggedTransform.localScale.x;
    }
    public void Initialize(Vector3 pos, float additionalHealth, bool bossMode)
    {
        base.Initialize();
        _health += additionalHealth;
        if (bossMode)
        {
            _health += 2 * additionalHealth;
            _riggedTransform.localScale = Vector3.one * _originalScale * 2f;
        }
        transform.position = pos;
        _rb.velocity = new Vector3(0, 0, -velocity);
    }
    protected override void HitVisuals()
    {
        StopCoroutine("hit");
        StartCoroutine("hit");
    }
    protected override void DeadVisuals()
    {
        var ps = _particlesPool.Pool.Get(); 
        if (_materials == null)
        {
            _materials = new Dictionary<Material, Color>();
            foreach (var material in _renderer.materials)
            {
                _materials.Add(material, material.color);
            }
        }
        ps.Initialize(transform.position, 0, _materials.First().Value);
    }
    private IEnumerator hit()
    {
        if(_materials == null)
        {
            _materials = new Dictionary<Material, Color>();
            foreach (var material in _renderer.materials)
            {
                _materials.Add(material, material.color);
            }
        }
        foreach (var mat in _materials.Keys)
        {
            mat.color = hitColor;
        }
        yield return new WaitForSeconds(0.2f);
        float duration = 0.3f;
        float elapsed = 0f;
        while (elapsed <= duration)
        {
            foreach (var mat in _materials.Keys)
            {
                mat.color = Color.Lerp(hitColor, _materials.GetValueOrDefault(mat), elapsed / duration);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}