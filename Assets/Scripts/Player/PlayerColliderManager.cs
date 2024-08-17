using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    public PlayerMainScript playerMainScript;
    public void ColliderTriggered(string name, string tagOfTrigger)
    {
        if (tagOfTrigger == "EnemyDamage") playerMainScript.PlayerTriggerDamage = true;
    }

}
