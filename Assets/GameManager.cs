using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject allyPool;  // Ally Pool object
    public GameObject enemyPool; // Enemy Pool object

    public float allyCurrentArmyCost = 0f;
    public float enemyCurrentArmyCost = 0f;

    public static GameManager instance;

    public float gameSpeed = 1f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate GameManager
            return;
        }

        instance = this;
        
        UpdateTotalGoldCost();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = gameSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTotalGoldCost();
    }

    public void UpdateTotalGoldCost()
    {
        allyCurrentArmyCost = 0f;
        enemyCurrentArmyCost = 0f;
        
        // assuming the pools are storing their entities as children
        foreach (Transform child in allyPool.transform) 
        {
            CharacterStatsHandler csh = child.GetComponent<CharacterStatsHandler>();
            if (csh != null)
            {
                allyCurrentArmyCost += csh.goldCost;
            }
        }

        foreach (Transform child in enemyPool.transform)
        {
            CharacterStatsHandler csh = child.GetComponent<CharacterStatsHandler>();
            if (csh != null)
            {
                enemyCurrentArmyCost += csh.goldCost;
            }
        }
    }
}
