using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    [SerializeField] TextMeshProUGUI timerText;


    void Start()
    {

        // Initialize the text to 0m 0s
        timerText.text = "0m 0s";
    }

    void Update()
    {
        // Increment the elapsed time
        elapsedTime += Time.deltaTime;

        // Convert the elapsed time to minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        // Update the timer text
        timerText.text = string.Format("{0}m {1}s", minutes, seconds);
    }
}
