using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TeamController teamController;

    // Define the projectileDamage variable
    public float projectileDamage = 10f; // Change this value as per your game balancing needs

    private void Start()
    {
        teamController = GetComponentInParent<TeamController>();
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        // If the projectile's team is Team1 and it hits an object on Team2, deal damage and destroy itself
        if (gameObject.layer == LayerMask.NameToLayer("Team 1") && collision.gameObject.layer == LayerMask.NameToLayer("Team 2"))
        {
            // Ignore anything with the Projectile tag
            if (collision.gameObject.tag == "Projectile")
            {
                return;
            }
            // Access the CharacterStatsHandler of the enemy and subtract the projectile damage from its health
            CharacterStatsHandler enemyStats = collision.gameObject.GetComponent<CharacterStatsHandler>();
            if(enemyStats != null)
            {
                enemyStats.health -= projectileDamage;
            }

            Destroy(gameObject);
        }
        // If the projectile's team is Team2 and it hits an object on Team1, deal damage and destroy itself
        else if (gameObject.layer == LayerMask.NameToLayer("Team 2") && collision.gameObject.layer == LayerMask.NameToLayer("Team 1"))
        {
            // Ignore anything with the Projectile tag
            if (collision.gameObject.tag == "Projectile")
            {
                return;
            }
            // Access the CharacterStatsHandler of the enemy and subtract the projectile damage from its health
            CharacterStatsHandler enemyStats = collision.gameObject.GetComponent<CharacterStatsHandler>();
            if(enemyStats != null)
            {
                enemyStats.health -= projectileDamage;
            }

            Destroy(gameObject);
        }
    }
}
