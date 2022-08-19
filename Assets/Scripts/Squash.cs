using System.Collections;
using UnityEngine;

public class Squash : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 100f)]
    private float maxVelocity = 80f;

    [SerializeField]
    [Range(0f, 2f)]
    private float maxSquashDuration = 0.3f;

    [SerializeField]
    [Range(0f, 1f)]
    private float maxSquashScaleModifier = 0.6f;

    [SerializeField]
    [Range(0f, 1f)]
    private float velocityDamping = 0.75f;

    private Rigidbody _rb;
    private Transform _blobTransform;
    private int _layerMask;
    private bool _coroutineRunning;

    #region CachedVariables

    float _intensity;
    float _fullDuration;
    float _firstPartDuration;
    float _secondPartDuration;
    float _elapsed;
    float _desiredModifier;

    Vector3 _normal;
    Vector3 _desiredScale;
    Vector3 _desiredPosition;
    Vector3 _lastVelocity;
    RaycastHit[] _hits;
    Collider[] _contactColliders;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _blobTransform = transform.GetChild(0);
        _coroutineRunning = false;

        // Default layer collision
        _layerMask = 1;
    }

    // For demo purposes
    private void Start()
    {
        RandomVelocity();
    }
    private void FixedUpdate()
    {
        // Velocity could be zero during coroutine
        if (_rb.velocity != Vector3.zero)
        {
            _lastVelocity = _rb.velocity;
        }

        _normal = Vector3.zero;

        // Detect overlapping colliders
        _contactColliders = Physics.OverlapSphere(transform.position, 0.5f, _layerMask);
        foreach (Collider coll in _contactColliders)
        {
            // Raycast to each overlapping point to get normals
            _hits = Physics.RaycastAll(transform.position,
                (coll.ClosestPoint(transform.position) - transform.position).normalized,
                Mathf.Infinity, _layerMask);

            // Make sure we are heading towards to the face by checking the dot product
            if (_hits.Length > 0 && Vector3.Dot(_lastVelocity, _hits[0].normal) < 0)
                _normal += _hits[0].normal;
        }
        _normal.Normalize();
        if (!_coroutineRunning && _normal != Vector3.zero)
        {
            StartCoroutine(SquashAndReflect());
        }
    }

    // For demo purposes
    private void OnDrawGizmos()
    {
        if (_contactColliders != null)
            foreach (var coll in _contactColliders)
            {
                Gizmos.DrawSphere(coll.ClosestPoint(transform.position), 0.15f);
                Debug.DrawLine(transform.position, coll.ClosestPoint(transform.position), Color.green);
            }

        Debug.DrawLine(transform.position, transform.position + _normal * 4f, Color.green);
    }
    private IEnumerator SquashAndReflect()
    {
        _coroutineRunning = true;

        // Determine intensity of how much the velocity is facing face we collided with
        _intensity = Mathf.Abs(Vector3.Dot(_lastVelocity.normalized, _normal)) * Mathf.Clamp01(_lastVelocity.magnitude / maxVelocity);

        // Assign values according to the intensity
        _fullDuration = Mathf.Lerp(0.01f, maxSquashDuration, _intensity);
        _desiredModifier = Mathf.Lerp(0, maxSquashScaleModifier, _intensity);

        // Cache the scale we are going to squash to and the according position offset
        _desiredScale = new Vector3(1 + _desiredModifier, 1 + _desiredModifier, _desiredModifier);
        _desiredPosition = Vector3.zero + (Vector3.back * _desiredModifier / 2f);

        // Stop the blob from moving temporarily to animate the squash
        _rb.velocity = Vector3.zero;
        _blobTransform.rotation = Quaternion.LookRotation(_normal);

        // We have two parts of the animation
        // The first one is the squashing and the second one is the inflation
        _firstPartDuration = _fullDuration * 0.75f;
        _secondPartDuration = _fullDuration * 0.25f;

        // Squash animation
        while (_elapsed < _firstPartDuration)
        {
            _elapsed += Time.deltaTime;
            _blobTransform.localScale = Vector3.Lerp(Vector3.one, _desiredScale, Mathf.Clamp01(_elapsed / _firstPartDuration));
            _blobTransform.localPosition = Vector3.Lerp(Vector3.zero, _desiredPosition, Mathf.Clamp01(_elapsed / _firstPartDuration));
            yield return null;
        }

        // Inflation animation
        _elapsed = 0;
        bool reflected = false;
        while (_elapsed < _secondPartDuration)
        {
            _elapsed += Time.deltaTime;
            _blobTransform.localScale = Vector3.Lerp(_desiredScale, Vector3.one, Mathf.Clamp01(_elapsed / _secondPartDuration));
            _blobTransform.localPosition = Vector3.Lerp(_desiredPosition, Vector3.zero, Mathf.Clamp01(_elapsed / _secondPartDuration));

            // Release Blob at half time
            if (!reflected && _elapsed >= _secondPartDuration * 0.5f)
            {
                _rb.velocity = Vector3.Reflect(_lastVelocity, _normal);

                // Move the object in the new velocity a little bit
                transform.position += _rb.velocity * Time.deltaTime;
                _coroutineRunning = false;
                reflected = true;
            }

            yield return null;
        }
    }
    Vector3 Reflect(Vector3 vec, Vector3 norm)
    {
        var result = vec - ((vec.x * norm.x + vec.y * norm.y + vec.z * norm.z) * 2 * norm);
        return result;
    }

    void RandomVelocity()
    {
        _rb.velocity = Vector3.down * 10f + Vector3.right * 5f + Vector3.back * 12f;
    }

}