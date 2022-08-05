using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobAnimator : MonoBehaviour
{

    IEnumerator wobbleCoroutine(Vector3 direction, float duration)
    {
        float elapsed = 0f;
        float d;
        direction = direction.normalized * 0.35f;
        direction.y = 0;
        Vector3 a = Vector3.zero + direction;
        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            d = Mathf.Clamp01(elapsed / duration);
            transform.localPosition = Vector3.LerpUnclamped(Vector3.zero, a, (1 - d) * Mathf.Sin(Mathf.PI * d * 4));
            yield return null;
        }
    }

    IEnumerator enrageCoroutine(float duration, float scaleUp)
    {
        float elapsed = 0f;
        Vector3 puffScale = Vector3.one * 1.4f * scaleUp;
        Vector3 originalScale = Vector3.one * 1.4f;
        transform.localScale = originalScale;
        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, puffScale, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
        elapsed = 0f;
        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
    }
    IEnumerator puffupCoroutine(float duration)
    {
        float elapsed = 0f;
        Vector3 originalScale = Vector3.one * 1.4f;

        transform.localScale = Vector3.zero;
        while (elapsed <= duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
    }
    public void Wobble(float duration, Vector3 direction)
    {
        StartCoroutine(wobbleCoroutine(direction, duration));
    }
    public void Enrage(float duration, float scaleUp)
    {
        StartCoroutine(enrageCoroutine(duration, scaleUp));
    }
    public void Puffup(float duration)
    {
        StartCoroutine(puffupCoroutine(duration));
    }

}
