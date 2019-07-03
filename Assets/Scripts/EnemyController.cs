using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int initialHealth;

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
        _currentHealth = initialHealth;
        GetComponent<Rigidbody2D>().mass = 0.13f * CurrentHealth;
    }

    /* Called when an object hits this */
    public void HitByObject(Vector2 velocity, float objectMass)
    {
        CurrentHealth -= (velocity.x * 7) * objectMass;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero || collision.collider.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(collision.collider.transform.position, Vector2.zero, 0f);

            foreach (RaycastHit2D hit in hits)
            {
            
                HitByObject(collision.collider.attachedRigidbody.velocity, hit.rigidbody.mass);
            }
        }
    }

}
