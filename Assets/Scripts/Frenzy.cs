using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : MonoBehaviour
{
    private EntityMovement entityMovement;  // The EntityMovement script
    public float switchTargetInterval = 2f;  // Time interval in seconds to switch target

    private void Start()
    {
        // Get the EntityMovement script
        entityMovement = GetComponent<EntityMovement>();

        // Call the SwitchTarget method repeatedly every 'switchTargetInterval' seconds
        InvokeRepeating(nameof(SwitchTarget), switchTargetInterval, switchTargetInterval);
    }

    private void Update()
    {
        // Make the entity move towards the new target
        if (entityMovement.closestEnemy != null)
        {
            entityMovement.MoveTowards(entityMovement.closestEnemy.transform.position);
        }
    }

    private void SwitchTarget()
    {
        // Get all TeamControllers in the scene
        TeamController[] controllers = FindObjectsOfType<TeamController>();

        List<TeamController> potentialTargets = new List<TeamController>();

        // Find all potential enemies
        foreach (TeamController controller in controllers)
        {
            // If the controller is on the opposite team and is not the current target
            if (controller.team != entityMovement.teamController.team &&
                controller.GetComponent<Collider2D>() != entityMovement.closestEnemy)
            {
                potentialTargets.Add(controller);
            }
        }

        // If there are potential targets, choose a random one
        if (potentialTargets.Count > 0)
        {
            int randomIndex = Random.Range(0, potentialTargets.Count);
            entityMovement.closestEnemy = potentialTargets[randomIndex].GetComponent<Collider2D>();
        }
    }
}
