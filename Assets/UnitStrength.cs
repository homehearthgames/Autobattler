using UnityEngine;
using UnityEngine.UI;

public class UnitStrength : MonoBehaviour
{
    public GameObject pool;  // Pool object (ally or enemy)
    public Image fillImage;  // Reference to the Image component

    public float currentArmyCost;
    public float maxArmyCost; // The maximum cost that the army can have
    public bool isAlly;  // Flag to differentiate between ally and enemy

    private void Awake() 
    {
    }

    private void Start()
    {
        GameManager.instance.UpdateTotalGoldCost();
        maxArmyCost = isAlly ? GameManager.instance.allyCurrentArmyCost : GameManager.instance.enemyCurrentArmyCost;

        if(isAlly) 
        {
            fillImage.color = Color.green; // Set ally bar color to green
        } 
        else 
        {
            fillImage.color = Color.red; // Set enemy bar color to red
        }
    }

    private void Update()
    {
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        currentArmyCost = isAlly ? GameManager.instance.allyCurrentArmyCost : GameManager.instance.enemyCurrentArmyCost;

        float fillAmount = currentArmyCost / maxArmyCost;
        fillImage.fillAmount = fillAmount;
    }
}
