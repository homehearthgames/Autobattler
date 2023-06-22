using System;
using UnityEngine;
// This script holds all of the stats for each character and allows
// them to be accessed from a single location
public class CharacterStatsHandler : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public float attackRange;
    [SerializeField] public float attackDamage;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float projectileSpeed;
    [SerializeField] public float projectileDamage;
    [SerializeField] public float rangedAttackSpeed;
    [SerializeField] public float goldCost;
    private GameManager gameManager;
    

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (characterStats != null)
        {
            goldCost = characterStats.GoldCost;
            health = characterStats.Health;
            maxHealth = health;
            speed = characterStats.Speed;
            attackRange = characterStats.AttackRange;
            attackDamage = characterStats.AttackDamage;
            attackSpeed = characterStats.AttackSpeed;
            projectileSpeed = characterStats.ProjectileSpeed;
            projectileDamage = characterStats.ProjectileDamage;
            rangedAttackSpeed = characterStats.RangedAttackSpeed;
        }
    }

    private void Update() {
        CheckHealth();
    }

private void CheckHealth()
{
    if (health <= 0)
    {
        // adjusting the cost based on which pool this character belongs to
        if (transform.parent.gameObject == gameManager.allyPool)
        {
            gameManager.allyCurrentArmyCost -= goldCost;
        }
        else if (transform.parent.gameObject == gameManager.enemyPool)
        {
            gameManager.enemyCurrentArmyCost -= goldCost;
        }
        Destroy(gameObject);
    }
}
}
