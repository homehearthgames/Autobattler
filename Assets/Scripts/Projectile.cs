using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TeamController teamController;
    private bool hasHit;  // Add this boolean flag

    public float projectileDamage = 10f;

    private void Start()
    {
        teamController = GetComponentInParent<TeamController>();
        Destroy(gameObject, 10f);
        hasHit = false;  // Initialize the flag as false
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If the projectile has already hit a target, do nothing
        if(hasHit)
            return;

        if (gameObject.layer == LayerMask.NameToLayer("Team 1") && collision.gameObject.layer == LayerMask.NameToLayer("Team 2"))
        {
            if (collision.gameObject.tag == "Projectile")
            {
                return;
            }

            CharacterStatsHandler enemyStats = collision.gameObject.GetComponent<CharacterStatsHandler>();
            if(enemyStats != null)
            {
                enemyStats.health -= projectileDamage;
                hasHit = true;  // Set the flag to true when the projectile hits a target
            }

            Destroy(gameObject);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Team 2") && collision.gameObject.layer == LayerMask.NameToLayer("Team 1"))
        {
            if (collision.gameObject.tag == "Projectile")
            {
                return;
            }

            CharacterStatsHandler enemyStats = collision.gameObject.GetComponent<CharacterStatsHandler>();
            if(enemyStats != null)
            {
                enemyStats.health -= projectileDamage;
                hasHit = true;  // Set the flag to true when the projectile hits a target
            }

            Destroy(gameObject);
        }
    }
}
