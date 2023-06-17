using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private float attackSpeed; // fires every Y seconds
    public GameObject weaponEffect; // projectile prefab
    public float weaponEffectOffset;
    public Sprite[] sprites; // sprites to be looped through

    private SpriteRenderer spriteRenderer; // reference to the SpriteRenderer
    private SpriteAnimator spriteAnimator; // reference to the SpriteAnimator
    private IEnumerator FireAnimationCoroutine;
    private AttackController attackController;
    private TeamController teamController;
    private CharacterStatsHandler characterStatsHandler;
    private float nextFire;

    private void Start()
    {
        attackController = GetComponent<AttackController>();
        teamController = GetComponent<TeamController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteAnimator = GetComponent<SpriteAnimator>();
        characterStatsHandler = GetComponent<CharacterStatsHandler>();

        attackSpeed = characterStatsHandler.attackSpeed;
    }

    private void Update()
    {
        if (attackController.isAttacking)
        {
            if (Time.time > nextFire)
            {
                // Start the animation
                if (FireAnimationCoroutine != null)
                {
                    
                    StopCoroutine(FireAnimationCoroutine);
                }

                FireAnimationCoroutine = AttackAnimation();
                StartCoroutine(FireAnimationCoroutine);

                nextFire = Time.time + attackSpeed;
            }
        }
        else
        {
            // Stop the animation and reset SpriteAnimator
            if (FireAnimationCoroutine != null)
            {
                StopCoroutine(FireAnimationCoroutine);
                FireAnimationCoroutine = null;
            }
            spriteAnimator.enabled = true;
            nextFire = 0;
        }
    }

    // The animation routine
    private IEnumerator AttackAnimation()
    {
        spriteAnimator.enabled = false;

        float delay = attackSpeed / sprites.Length;
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteRenderer.sprite = sprites[i];
            yield return new WaitForSeconds(delay);

            // Fire projectile after the last sprite has been displayed
            if (i == 2)
            {
                Fire();
            }
        }

        spriteAnimator.enabled = true;
    }



private void Fire()
{
    EntityMovement entityMovement = GetComponent<EntityMovement>();
    if (entityMovement.closestEnemy == null) // Add this line to check if the enemy still exists
    {
        // If the enemy does not exist, don't execute the rest of the code
        return;
    }
    Vector2 direction = (attackController.entityMovement.closestEnemy.transform.position - transform.position).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    // Instantiate the projectile about .5 units away from the center of the game object, but toward the enemyCharacterStatsHandler gameobject.
    Vector3 spawnPosition = transform.position + (Vector3)(weaponEffectOffset * direction);
    // GameObject newWeaponEffect = Instantiate(weaponEffect, spawnPosition, Quaternion.Euler(0, 0, angle));

    CharacterStatsHandler enemyCharacterStatsHandler = entityMovement.closestEnemy.GetComponent<CharacterStatsHandler>();

    enemyCharacterStatsHandler.health -= this.characterStatsHandler.attackDamage; 



    // Assign the projectile to the same layer as the object that fires it.
    // newWeaponEffect.layer = gameObject.layer;
}


}
