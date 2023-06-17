using System.Collections;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public float projectileSpeed = 10f; // X speed
    public float fireRate = 2f; // fires every Y seconds
    public GameObject projectile; // projectile prefab
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

        projectileSpeed = characterStatsHandler.projectileSpeed;
        fireRate = characterStatsHandler.rangedAttackSpeed;
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

                nextFire = Time.time + fireRate;
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

        float delay = fireRate / sprites.Length;
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
    if (attackController.entityMovement.closestEnemy != null)
    {
        Vector2 direction = (attackController.entityMovement.closestEnemy.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Instantiate the projectile facing the direction it's moving in.
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, angle));
        Projectile projectileInstance = newProjectile.GetComponent<Projectile>();
        projectileInstance.projectileDamage = characterStatsHandler.projectileDamage;
        // Set the velocity so that the projectile moves towards the center of the enemy.
        direction *= projectileSpeed;

        newProjectile.GetComponent<Rigidbody2D>().velocity = direction;

        // Assign the projectile to the same layer as the object that fires it.
        newProjectile.layer = gameObject.layer;
    }
    else
    {
        // Handle case when closest enemy is null
    }
}


}
