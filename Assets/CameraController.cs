using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Vector3 startScale; // initial scale
    private Vector3 targetScale; // target scale
    public float scaleTime = 5f; // time for the scale operation to take place

    private void Start() {
        startScale = new Vector3(1.25f, 1.25f, 1.25f); // initial scale
        targetScale = Vector3.one; // target scale
        transform.localScale = startScale; // set the initial scale

        // Start the coroutine to perform the scale
        StartCoroutine(ScaleCamera(startScale, targetScale, scaleTime));
    }

    IEnumerator ScaleCamera(Vector3 startScale, Vector3 targetScale, float duration) {
        float elapsed = 0; // elapsed time counter

        // While the elapsed time is less than the duration of the scale
        while (elapsed < duration) {
            // Increase the elapsed time by the time since the last frame
            elapsed += Time.deltaTime;

            // Calculate the current scale using SmoothStep for a smoother transition
            Vector3 currentScale = Vector3.Lerp(startScale, targetScale, Mathf.SmoothStep(0.0f, 1.0f, elapsed / duration));

            // Apply the scale to the transform
            transform.localScale = currentScale;

            yield return null; // Wait until the next frame before continuing the loop
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = targetScale;
    }
}
