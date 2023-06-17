using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Team1,
    Team2
}

public class TeamController : MonoBehaviour
{
    public Team team;

    // Start is called before the first frame update
    void Start()
    {
        if (team == Team.Team1)
        {
            // Assign the object to the Team1 layer
            gameObject.layer = LayerMask.NameToLayer("Team 1");
        }
        else if (team == Team.Team2)
        {
            // Assign the object to the Team2 layer
            gameObject.layer = LayerMask.NameToLayer("Team 2");
        }
    }
}
