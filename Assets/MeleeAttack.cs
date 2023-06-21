using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private float attackSpeed; // fires every Y seconds
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

        CharacterStatsHandler enemyCharacterStatsHandler = entityMovement.closestEnemy.GetComponent<CharacterStatsHandler>();

        enemyCharacterStatsHandler.health -= this.characterStatsHandler.attackDamage; 
    }


}
