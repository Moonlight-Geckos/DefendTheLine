using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Target
{
    [SerializeField]
    private float velocity;

    [SerializeField]
    private Color hitColor = Color.white;

    private Rigidbody _rb;
    private Dictionary<Material, Color> _materials;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var renderer = GetComponentInChildren<Renderer>();

        _materials = new Dictionary<Material, Color>();
        foreach (var material in renderer.materials)
        {
            _materials.Add(material, material.color);
        }
    }
    public void Initialize(Vector3 pos, float additionalHealth)
    {
        base.Initialize();
        _health += additionalHealth;
        transform.position = pos;
        _rb.velocity = new Vector3(0, 0, -velocity);
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
        // To be implemented
    }
    private IEnumerator hit()
    {
        yield return null;
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