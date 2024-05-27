using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputRebind : MonoBehaviour
{
    //********** PRIVATE **********
    //***** References *****
    private PlayerMovementScript movementScript;
    private void Awake()
    {
        movementScript = FindObjectOfType<PlayerMovementScript>();
        
    }
}
