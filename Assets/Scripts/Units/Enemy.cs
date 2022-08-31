using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Target
{
    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private float minVelocity;

    [SerializeField]
    private float rotationVelocity = 2f;

    [SerializeField]
    private Color hitColor = Color.white;

    private ParticlesPool _particlesPool;
    private Dictionary<Material, Color> _materials;
    private Transform _riggedTransform;
    private Renderer _renderer;
    protected Rigidbody _rb;
    protected Vector3 direction;
    private float _originalScale;
    private float _velocity;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<Renderer>();
        _particlesPool = PoolsPool.Instance.StickmenExplosionPool;
        _riggedTransform = transform.Find("RiggedMesh");
        _originalScale = _riggedTransform.localScale.x;

        _materials = new Dictionary<Material, Color>();
        foreach (var material in _renderer.materials)
        {
            _materials.Add(material, material.color);
        }
    }
    private void FixedUpdate()
    {
        _velocity = Mathf.Max(_velocity - Time.fixedDeltaTime * 0.02f, minVelocity);
        _rb.MoveRotation(Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(direction, Vector3.up), Time.fixedDeltaTime * rotationVelocity));
        _rb.velocity = transform.forward * _velocity;
    }
    public void Initialize(Vector3 pos, Vector3 direction, float additionalHealth, bool bossMode)
    {
        base.Initialize();

        _velocity = maxVelocity;
        transform.position = pos;
        transform.forward = direction;
        this.direction = direction;
        _rb.velocity = direction * _velocity;

        _health += additionalHealth;
        if (bossMode)
        {
            _health += 2 * additionalHealth;
            _riggedTransform.localScale = Vector3.one * _originalScale * 2f;
        }
        foreach (var mat in _materials.Keys)
        {
            mat.color = _materials.GetValueOrDefault(mat);
        }
    }
    protected override void HitVisuals()
    {
        StopCoroutine("hit");
        StartCoroutine("hit");
    }
    protected override void DeadVisuals()
    {
        var ps = _particlesPool.Pool.Get(); 
        ps.Initialize(transform.position, 0, _materials.First().Value);
    }
    private IEnumerator hit()
    {
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
    protected override void TurnTo(Transform other, Vector3 direction)
    {
        this.direction = direction;
    }
}