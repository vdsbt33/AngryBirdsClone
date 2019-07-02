using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /* Detects collisions between this object and another one */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /* If this object is the player bullet */
        if (playerController.State != PlayerController.BulletState.Aiming) { 
            playerController.State = PlayerController.BulletState.Collided;
        }
    }
}
