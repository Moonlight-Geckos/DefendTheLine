using System.Collections;
using UnityEngine;

public class BlobAnimator : MonoBehaviour
{
    private bool _coroutineRunning;
    private IEnumerator SquashAndReflect(Vector3 normal)
    {
        _coroutineRunning = true;

        var _fullDuration = 0.1f;
        var _desiredModifier = 0.75f;

        // Look at the direction of the normal
        transform.rotation = Quaternion.LookRotation(normal);

        // Cache the scale we are going to squash to and the according position offset
        var _desiredScale = new Vector3(2 - _desiredModifier, 2 - _desiredModifier, _desiredModifier);
        var _desiredPosition = Vector3.zero + (-transform.forward * (1 - _desiredModifier));


        // We have two parts of the animation
        // The first one is the squashing and the second one is the inflation
        var _firstPartDuration = _fullDuration * 0.75f;
        var _secondPartDuration = _fullDuration * 0.25f;

        float elapsed = 0;

        // Squash animation
        while (elapsed < _firstPartDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, _desiredScale, Mathf.Clamp01(elapsed / _firstPartDuration));
            transform.localPosition = Vector3.Lerp(Vector3.zero, _desiredPosition, Mathf.Clamp01(elapsed / _firstPartDuration));
            yield return null;
        }

        elapsed = 0;

        // Inflation animation
        while (elapsed < _secondPartDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(_desiredScale, Vector3.one, Mathf.Clamp01(elapsed / _secondPartDuration));
            transform.localPosition = Vector3.Lerp(_desiredPosition, Vector3.zero, Mathf.Clamp01(elapsed / _secondPartDuration));
            _coroutineRunning = false;

            yield return null;
        }
    }
    public void Squash(Vector3 normal)
    {
        if(!_coroutineRunning)
            StartCoroutine(SquashAndReflect(normal));
    }

}
