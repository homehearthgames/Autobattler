using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private CharacterStatsHandler characterStatsHandler;
    public Image healthBarImage; // This is the one that goes down instantly
    public Image lazyHealthBarImage; // This is the one that goes down smoothly
    public Image healthBarBackground;
    public TeamController teamController;
    private bool isHealthBarVisible;

    private float lazyFillAmount; // To keep track of the lazy health bar fill amount

    private void Awake()
    {
        characterStatsHandler = GetComponentInParent<CharacterStatsHandler>();
        teamController = GetComponentInParent<TeamController>();
    }

    private void Start()
    {
        // Disable the health bar image initially
        healthBarImage.enabled = false;
        lazyHealthBarImage.enabled = false;
        healthBarBackground.enabled = false;
        isHealthBarVisible = false;
        lazyFillAmount = 1f; // Initial fill amount for lazy health bar
    }

    private void Update()
    {
        // Ensure that the required references are set
        if (characterStatsHandler == null || healthBarImage == null || lazyHealthBarImage == null || teamController == null)
        {
            Debug.LogWarning("References not set for HealthBar script.");
            return;
        }

        // Calculate the fill amount based on the current health and maximum health
        float fillAmount = characterStatsHandler.health / characterStatsHandler.maxHealth;

        // Update the fill amount of the lazy health bar
        lazyFillAmount = Mathf.Lerp(lazyFillAmount, fillAmount, 0.005f); // 0.1f controls the speed
        lazyHealthBarImage.fillAmount = lazyFillAmount;

        // Update the fill amount of the health bar image instantly
        healthBarImage.fillAmount = fillAmount;

        // Check if the health bar should be visible
        if (!isHealthBarVisible && fillAmount < 1f)
        {
            // Enable the health bar image
            healthBarImage.enabled = true;
            lazyHealthBarImage.enabled = true;
            healthBarBackground.enabled = true;
            isHealthBarVisible = true;
        }
        else if (isHealthBarVisible && fillAmount >= 1f)
        {
            // Disable the health bar image
            healthBarImage.enabled = false;
            lazyHealthBarImage.enabled = false;
            healthBarBackground.enabled = false;
            isHealthBarVisible = false;
        }

        // Set the color of the health bar fill based on the team
        if (teamController.team == Team.Team1)
        {
            healthBarImage.color = Color.green;
            lazyHealthBarImage.color = new Color(0, 1, 0, 0.5f); // Slightly transparent green
        }
        else if (teamController.team == Team.Team2)
        {
            healthBarImage.color = Color.red;
            lazyHealthBarImage.color = new Color(1, 0, 0, 0.5f); // Slightly transparent red
        }
    }
}
