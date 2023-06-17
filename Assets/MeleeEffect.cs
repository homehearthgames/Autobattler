using System.Collections;
using UnityEngine;

public class MeleeEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeTime = .25f;

    private void Start()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator FadeOutAndDestroy()
    {
        float startAlpha = spriteRenderer.color.a;

        float rate = 1.0f / fadeTime;

        for (float i = 0; i <= fadeTime; i += Time.deltaTime)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(startAlpha, 0, i * rate));
            spriteRenderer.color = newColor;
            yield return null;
        }

        Destroy(gameObject);
    }
}
