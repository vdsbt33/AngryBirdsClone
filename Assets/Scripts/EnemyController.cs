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

    /* The minimum amount of velocity needed until the target takes damage */
    public float damageMinimumVelocityMagnitude = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = initialHealth;
        GetComponent<Rigidbody2D>().mass = 0.13f * CurrentHealth;
    }

    /* Called when an object hits this */
    public void HitByObject(Vector2 objectVelocity, float objectMass)
    {
        print(string.Format("Previous HP: {0} | New HP: {1} | Total DMG: {2}", CurrentHealth, CurrentHealth - ((objectMass * objectVelocity).magnitude + (GetComponent<Rigidbody2D>().mass * GetComponent<Rigidbody2D>().velocity).magnitude) * 3, ((objectMass * objectVelocity).magnitude + (GetComponent<Rigidbody2D>().mass * GetComponent<Rigidbody2D>().velocity).magnitude) * 3));
        CurrentHealth -= ((objectMass * objectVelocity).magnitude + (GetComponent<Rigidbody2D>().mass * GetComponent<Rigidbody2D>().velocity).magnitude) * 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Rigidbody2D>().velocity.magnitude > damageMinimumVelocityMagnitude || collision.collider.GetComponent<Rigidbody2D>().velocity.magnitude > damageMinimumVelocityMagnitude)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(collision.collider.transform.position, Vector2.zero, 0f);

            foreach (RaycastHit2D hit in hits)
            {
                HitByObject(collision.collider.attachedRigidbody.velocity, hit.rigidbody.mass);
            }
        }
    }

}
