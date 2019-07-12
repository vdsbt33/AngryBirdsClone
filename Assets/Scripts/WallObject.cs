using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    public enum MaterialType
    {
        Wood,
        Stone
    }
    public MaterialType materialType;

    public enum FreezeType
    { 
        Normal = 0,
        FrozenPermanently = 1,
        FrozenUntilCollided = 2
    }

    public FreezeType freezeType;

    private int _initialHealth
    {
        get
        {
            if (materialType == MaterialType.Wood)
            {
                return 10;
            } else if (materialType == MaterialType.Stone)
            {
                return 20;
            }
            return -1;
        }
    }

    private float _currentHealth;
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            if (value > 0)
            {
                _currentHealth = value;
                return;
            }
            Destroy(gameObject, 0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _initialHealth;
        GetComponent<Rigidbody2D>().mass = 0.13f * CurrentHealth;
    }

    /* Called when an object hits this */
    public void HitByObject(Vector2 velocity, float objectMass)
    {
        CurrentHealth -= (velocity.x * 7) * objectMass;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll(collision.collider.transform.position, Vector2.zero, 0f);
        //print("Collider name: " + collision.collider.name + " |otherCollider name: " + collision.otherCollider.name);
        foreach (RaycastHit2D hit in hits)
        {
            //print("GameObject name: " + hit.transform.gameObject.name);
            HitByObject(collision.collider.attachedRigidbody.velocity, hit.rigidbody.mass);
        }
        
    }

}
