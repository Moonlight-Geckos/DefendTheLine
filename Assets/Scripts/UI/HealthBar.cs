using System.Collections;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private float animationLength = 1f;

    [SerializeField]
    private float visibilityDuration = 2f;

    private ProgressBar healthBar;
    private CanvasGroup canvasGroup;

    private Timer timer;
    private void Awake()
    {
        healthBar = GetComponentInChildren<ProgressBar>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateValue(float val)
    {
        healthBar.UpdateValue(val);
        void ChangeColor()
        {
            IEnumerator changeColor()
            {
                yield return null;
                canvasGroup.alpha = 1;
            }
            StartCoroutine(changeColor());
        }
        void ResetColors()
        {
            IEnumerator changeColor()
            {
                float alpha = canvasGroup.alpha;
                while (canvasGroup.alpha > 0)
                {
                    alpha -= Time.deltaTime / animationLength;
                    canvasGroup.alpha = alpha;
                    yield return new WaitForEndOfFrame();
                }
            }
            StartCoroutine(changeColor());
        }

        if (timer == null)
        {
            timer = TimersPool.Instance.Pool.Get();
            timer.Duration = visibilityDuration;
            timer.AddTimerFinishedEventListener(ResetColors);
        }

        if (timer.Running)
        {
            StopAllCoroutines();
            timer.Refresh();
        }
        ChangeColor();
        timer.Run();
    }

    public void ResetEffect()
    {
        StopAllCoroutines();
        timer?.Stop();
        canvasGroup.alpha = 0f;
    }
}
