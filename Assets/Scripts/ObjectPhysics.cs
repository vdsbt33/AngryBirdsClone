using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysics : MonoBehaviour
{
    public PlayerController playerController;
    public BulletAmmo bulletAmmo;

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
        /* There's a bug in which, sometimes, player won't go back to Waiting
         * Find what's causing it and fix it
         */
        if (playerController.State != PlayerController.BulletState.Aiming) {
            playerController.State = PlayerController.BulletState.Collided;
        }
    }
}
