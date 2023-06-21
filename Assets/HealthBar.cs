using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private CharacterStatsHandler characterStatsHandler;
    public Image healthBarImage;
    public Image healthBarBackground;
    public TeamController teamController;
    private bool isHealthBarVisible;

    private void Awake()
    {
        characterStatsHandler = GetComponent<CharacterStatsHandler>();
        teamController = GetComponent<TeamController>();
    }

    private void Start()
    {
        // Disable the health bar image initially
        healthBarImage.enabled = false;
        healthBarBackground.enabled = false;
        isHealthBarVisible = false;
    }

    private void Update()
    {
        // Ensure that the required references are set
        if (characterStatsHandler == null || healthBarImage == null || teamController == null)
        {
            Debug.LogWarning("References not set for HealthBar script.");
            return;
        }

        // Calculate the fill amount based on the current health and maximum health
        float fillAmount = characterStatsHandler.health / characterStatsHandler.maxHealth;

        // Update the fill amount of the health bar image
        healthBarImage.fillAmount = fillAmount;

        // Check if the health bar should be visible
        if (!isHealthBarVisible && fillAmount < 1f)
        {
            // Enable the health bar image
            healthBarImage.enabled = true;
            healthBarBackground.enabled = true;
            isHealthBarVisible = true;
        }
        else if (isHealthBarVisible && fillAmount >= 1f)
        {
            // Disable the health bar image
            healthBarImage.enabled = false;
            healthBarBackground.enabled = false;
            isHealthBarVisible = false;
        }

        // Set the color of the health bar fill based on the team
        if (teamController.team == Team.Team1)
        {
            healthBarImage.color = Color.green;
        }
        else if (teamController.team == Team.Team2)
        {
            healthBarImage.color = Color.red;
        }
    }
}
