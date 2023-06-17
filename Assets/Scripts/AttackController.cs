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
        if (entityMovement.isInContact || IsEnemyWithinAttackRange())
        {
            isAttacking = true;
            AttackEnemy();
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

    private void AttackEnemy()
    {
        // Insert your attack logic here
        // Debug.Log("Attacking");
        // stop the character from playing its movement animation
        // start the attack animation ...

        // we need to break out of here when the enemy gets out of range
    }
}
