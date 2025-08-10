using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraSnapEffect : MonoBehaviour
{
    [Header("Refs")]
    public CanvasGroup flash; // Flash overlay (white), alpha=0
    public RectTransform topShutter; // Start offscreen top
    public RectTransform bottomShutter; // Start offscreen bottom
    public AudioSource shutterSfx; // Optional

    [Header("Timings")]
    public float shutterIn = 0.08f; // close
    public float hold = 0.05f; // closed pause
    public float shutterOut = 0.10f; // open
    public float flashUp = 0.04f; // flash in
    public float flashDown = 0.15f; // flash out

    Vector2 topStart,
        bottomStart;
    Vector2 topClosed,
        bottomClosed;

    void Awake()
    {
        // Cache start positions (offscreen)
        topStart = topShutter.anchoredPosition;
        bottomStart = bottomShutter.anchoredPosition;

        // Closed position = meet at center (y = 0)
        topClosed = new Vector2(topStart.x, 0f);
        bottomClosed = new Vector2(bottomStart.x, 0f);

        flash.alpha = 0f;
    }

    public void PlaySnap()
    {
        StopAllCoroutines();
        StartCoroutine(SnapRoutine());
    }

    IEnumerator SnapRoutine()
    {
        // (optional) play sound right as the shutter begins
        if (shutterSfx)
            shutterSfx.Play();

        // Flash up while shutters close
        StartCoroutine(FlashRoutine());

        // Close
        yield return MoveRT(topShutter, topStart, topClosed, shutterIn);
        yield return MoveRT(bottomShutter, bottomStart, bottomClosed, shutterIn);

        yield return new WaitForSeconds(hold);

        // Open
        yield return MoveRT(topShutter, topClosed, topStart, shutterOut);
        yield return MoveRT(bottomShutter, bottomClosed, bottomStart, shutterOut);
    }

    IEnumerator FlashRoutine()
    {
        // quick white pop, then fade
        float t = 0f;
        while (t < flashUp)
        {
            t += Time.deltaTime;
            flash.alpha = Mathf.Infinity == 0 ? 1f : Mathf.Lerp(0f, 1f, t / flashUp);
            yield return null;
        }
        t = 0f;
        while (t < flashDown)
        {
            t += Time.deltaTime;
            flash.alpha = Mathf.Lerp(1f, 0f, t / flashDown);
            yield return null;
        }
        flash.alpha = 0f;
    }

    IEnumerator MoveRT(RectTransform rt, Vector2 from, Vector2 to, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float u = Mathf.SmoothStep(0f, 1f, t / dur);
            rt.anchoredPosition = Vector2.LerpUnclamped(from, to, u);
            yield return null;
        }
        rt.anchoredPosition = to;
    }
}
