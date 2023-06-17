using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 2f;  // Maximum speed at which character moves
    public float acceleration = 0.5f;  // Acceleration of the character
    private float currentSpeed = 0f; // Current speed of the character
    public float attackRange = 2f;  // Range within which the character can attack
    public float targetSwitchInterval = 5f;  // Interval at which the random target switches
    public TeamController teamController;  // The TeamController attached to this character
    private SpriteRenderer spriteRenderer;  // The SpriteRenderer component
    public Collider2D closestEnemy;  // The closest enemy collider
    private CharacterStatsHandler characterStatsHandler;
    private float lastTargetSwitchTime;  // The time when the last target switch occurred

    public float overlapAvoidanceForce = 1f;  // The force to move the character away when it overlaps with a teammate



    public bool isInContact = false;  // Is the character in contact with an enemy
    public bool randomTargetingEnabled = false;  // Is random targeting enabled

private void Start()
{
    // Get the TeamController and SpriteRenderer components
    teamController = GetComponent<TeamController>();
    spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    characterStatsHandler = GetComponent<CharacterStatsHandler>();

    SetStats();

    // Set the initial time for the last target switch
    lastTargetSwitchTime = Time.time;

    // Find the closest enemy
    FindClosestEnemy();

    // Add flipping logic here
    // Check if there is a closest enemy
    if (closestEnemy != null)
    {
        // Calculate the direction to the enemy
        Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;

        // Flip the sprite based on direction
        if (direction.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
    }
}



    private void SetStats()
    {
        speed = characterStatsHandler.speed;
        attackRange = characterStatsHandler.attackRange;
    }

    private void Update()
    {
        // If the character is in contact with an enemy, attack and don't move
        // if (isInContact)
        // {
        //     return;
        // }

        if (closestEnemy == null && randomTargetingEnabled)
        {
            
            currentSpeed = 0f; // Reset speed when no contact with enemy
            SwitchRandomTarget();
        }

        // Check if random targeting is enabled and it's time to switch the random target
        // if (randomTargetingEnabled && Time.time - lastTargetSwitchTime >= targetSwitchInterval)
        // {
        //     SwitchRandomTarget();
        // }

        else if (!randomTargetingEnabled)
        {
            // Find the closest enemy
            FindClosestEnemy();
        }

        // If no enemy found, do nothing (one-line if statement)
        if (closestEnemy == null) return;

        // Calculate the distance to the enemy
        float distanceToEnemy = Vector2.Distance(transform.position, closestEnemy.transform.position);

        // Check if the enemy is within the attack range
        if (distanceToEnemy <= attackRange)
        {
            return;
        }
        else
        {
            MoveTowards(closestEnemy.transform.position);
        }
        AdjustForOverlap();
    }
private void AdjustForOverlap()
{
    // Get all TeamControllers in the scene
    TeamController[] controllers = FindObjectsOfType<TeamController>();

    // Threshold for distance between character centers
    float centerDistanceThreshold = 1f;

    // Iterate over each TeamController
    foreach (TeamController controller in controllers)
    {
        // If the controller is on the same team and it's not the character itself
        if (controller.team == teamController.team && controller != teamController)
        {
            // Calculate the distance between character centers
            float centerDistance = Vector2.Distance(transform.position, controller.transform.position);

            // If the distance is less than the threshold
            if (centerDistance < centerDistanceThreshold)
            {
                // Calculate the direction to move away
                Vector2 awayFromTeammate = (Vector2)transform.position - (Vector2)controller.transform.position;
                awayFromTeammate.Normalize();

                // Move the character away from the teammate
                transform.position = (Vector2)transform.position + awayFromTeammate * overlapAvoidanceForce * Time.deltaTime;
            }
        }
    }
}

    private void FindClosestEnemy()
    {
        // Get all TeamControllers in the scene
        TeamController[] controllers = FindObjectsOfType<TeamController>();

        float closestDistance = Mathf.Infinity;
        Collider2D newClosestEnemy = null;

        // Find the closest enemy
        foreach (TeamController controller in controllers)
        {
            // If the controller is on the opposite team
            if (controller.team != teamController.team)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, controller.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    newClosestEnemy = controller.GetComponent<Collider2D>();
                }
            }
        }

        // Update the closest enemy only if it's not null
        if (newClosestEnemy != null)
        {
            closestEnemy = newClosestEnemy;
        }
    }

    private void SwitchRandomTarget()
    {
        // Get all TeamControllers in the scene
        TeamController[] controllers = FindObjectsOfType<TeamController>();

        List<Collider2D> enemies = new List<Collider2D>();

        // Find all enemies
        foreach (TeamController controller in controllers)
        {
            // If the controller is on the opposite team
            if (controller.team != teamController.team)
            {
                enemies.Add(controller.GetComponent<Collider2D>());
            }
        }

        // Check if there are any enemies
        if (enemies.Count > 0)
        {
            // Select a random enemy as the new target
            int randomIndex = UnityEngine.Random.Range(0, enemies.Count);
            closestEnemy = enemies[randomIndex];

            // Update the last target switch time
            lastTargetSwitchTime = Time.time;
        }
        else
        {
            closestEnemy = null;
        }
    }

    public void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;

        // Accelerate
        currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, speed);

        // Flip the sprite based on direction
        if (direction.x > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0f)
        {
            spriteRenderer.flipX = true;
        }

        // Move towards the target
        transform.position += (Vector3)direction * currentSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is an enemy
        TeamController enemyTeamController = other.GetComponent<TeamController>();
        if (enemyTeamController != null && enemyTeamController.team != teamController.team)
        {
            isInContact = true;
        }
    }

    // In OnTriggerExit2D method, reset current speed
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other collider is an enemy
        TeamController enemyTeamController = other.GetComponent<TeamController>();
        if (enemyTeamController != null && enemyTeamController.team != teamController.team)
        {
            isInContact = false;
        }
    }
}
