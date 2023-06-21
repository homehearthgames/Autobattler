using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum UnitType
{
    Ranged,
    Melee,
    Cavalry
}
public class EntityMovement : MonoBehaviour
{
    public UnitType unitType;  // The type of unit
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
        FindInitialEnemy();

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

    private void FixedUpdate()
    {

        if (closestEnemy == null)
        {
            // Find a random nearby enemy
            FindNearbyEnemy();
        
            // If no enemy found, do look for the closest enemy.
            if (closestEnemy == null)
            {
                FindClosestEnemy();

                if (closestEnemy == null)
                {
                    return;
                }
            }
        }

        // Calculate the distance to the enemy
        float distanceToEnemy = Vector2.Distance(transform.position, closestEnemy.transform.position);

        // Check if the enemy is within the attack range
        if (distanceToEnemy <= attackRange && closestEnemy != null)
        {
            return;
        }
        
        // if (distanceToEnemy > attackRange + 3)
        // {
        //     FindClosestEnemy();
        // }

        MoveTowards(closestEnemy.transform.position);
        AdjustForOverlap();
    }

    private void AdjustForOverlap()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, .5f);

        foreach (Collider2D collider in hitColliders)
        {
            TeamController otherTeamController = collider.GetComponent<TeamController>();

            if (otherTeamController != null && otherTeamController.team == teamController.team && collider != teamController.GetComponent<Collider2D>())
            {
                float centerDistance = Vector2.Distance(transform.position, collider.transform.position);

                if (centerDistance < .5f)
                {
                    Vector2 awayFromTeammate = (Vector2)transform.position - (Vector2)collider.transform.position;
                    awayFromTeammate.Normalize();

                    transform.position += (Vector3)(awayFromTeammate * overlapAvoidanceForce * Time.deltaTime);
                }
            }
        }
    }

    private void FindClosestEnemy()
    {

        Debug.Log("Finding Closest Enemy.");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange + 64);

        float closestDistance = Mathf.Infinity;
        Collider2D newClosestEnemy = null;

        foreach (Collider2D collider in hitColliders)
        {
            TeamController teamController = collider.GetComponent<TeamController>();

            if (teamController != null && teamController.team != this.teamController.team)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, collider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    newClosestEnemy = collider;
                }
            }
        }

        closestEnemy = newClosestEnemy;
    }
    
    private void FindNearbyEnemy()
    {
        Debug.Log("Finding Nearby Enemy.");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange + 2);

        List<Collider2D> enemyColliders = new List<Collider2D>();

        foreach (Collider2D collider in hitColliders)
        {
            TeamController teamController = collider.GetComponent<TeamController>();

            if (teamController != null && teamController.team != this.teamController.team)
            {
                enemyColliders.Add(collider);
            }
        }

        if (enemyColliders.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemyColliders.Count);
            closestEnemy = enemyColliders[randomIndex];
        }
        else
        {
            closestEnemy = null;
        }
    }

private void FindInitialEnemy()
{
    Debug.Log("Finding Initial Enemy.");
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange + 64);

    List<Collider2D> enemiesInRange = new List<Collider2D>();

    foreach (Collider2D collider in hitColliders)
    {
        TeamController teamController = collider.GetComponent<TeamController>();

        if (teamController != null && teamController.team != this.teamController.team)
        {
            enemiesInRange.Add(collider);
        }
    }

    // Sort the enemies by their distance to this entity
    enemiesInRange.Sort((x, y) => 
    {
        float distX = Vector2.Distance(transform.position, x.transform.position);
        float distY = Vector2.Distance(transform.position, y.transform.position);
        return distX.CompareTo(distY);
    });

    // Take the 5 closest enemies, or all if there are less than 5
    List<Collider2D> closestEnemies = enemiesInRange.Take(Math.Min(5, enemiesInRange.Count)).ToList();

    // If there are any enemies in range
    if (closestEnemies.Count > 0)
    {
        // Choose a random one
        int randomIndex = UnityEngine.Random.Range(0, closestEnemies.Count);
        closestEnemy = closestEnemies[randomIndex];
    }
    else
    {
        closestEnemy = null;
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
