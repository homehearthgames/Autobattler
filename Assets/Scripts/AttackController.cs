using UnityEngine;

public class AttackController : MonoBehaviour
{
    public EntityMovement entityMovement;  // The EntityMovement script attached to this character
    public bool isAttacking = false;  // Is the character attacking

    private void Start()
    {
        // Get the EntityMovement script
        entityMovement = GetComponent<EntityMovement>();
    }

    private void Update()
    {
        // If the character is in contact with an enemy or an enemy is within attack range, set isAttacking to true and call the attack function
        if (IsEnemyWithinAttackRange())
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }
    }

    private bool IsEnemyWithinAttackRange()
    {
        if (entityMovement.closestEnemy == null) 
            return false;

        // Calculate the distance to the enemy
        float distanceToEnemy = Vector2.Distance(transform.position, entityMovement.closestEnemy.transform.position);
        // Check if the enemy is within the attack range
        return distanceToEnemy <= entityMovement.attackRange;
    }
}
