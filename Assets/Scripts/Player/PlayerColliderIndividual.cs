using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderIndividual : MonoBehaviour
{
    public string colliderName;
    [SerializeField] PlayerColliderManager manager;

    private void Start()
    {
        if (manager == null) Debug.Log("!ERROR! Could Not Find PlayerColliderManager");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        manager.ColliderTriggered(colliderName, collision.tag);
    }


}
