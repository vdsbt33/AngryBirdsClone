using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /* Public variables */
    public Vector2 startingPoint;
    private Transform playerBullet; // Currently selected Player Bullet

    public Vector2 shootPositionOffset = new Vector2(0, 0);
    public float mouseDragRadius = 3f;
    public LayerMask playerSelectedAmmo;
    public float dragSmoothness = 1.0f;
    public float bulletForceMultiplier = 1.0f;
    public float gravityScale = 0.7f;
    public float resetWaitInSeconds = 2f;

    public List<GameObject> bulletList;
    private int startingProjectileCount;

    private GameObject GetCurrentBullet
    {
        get
        {
            return bulletList.Count > 0 ? bulletList[0] : null;
        }
    }

    public GameController gameController;

    public enum BulletState
    {
        Waiting = 0,
        Aiming = 1,
        Launching = 2,
        Collided = 3
    }

    private BulletState _state;
    public BulletState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (value == BulletState.Waiting)
            {
                print("Should have reset");
            } else if (value == BulletState.Collided)
            {
                waitTime = Time.time;
                playerBullet.GetComponent<Animator>().SetBool("isLaunching", false);
            }
            _state = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        startingProjectileCount = bulletList.Count;
        PostShot();
        waitTime = Time.time;
    }

    /* Gets player click/hold */
    void Update()
    {
        if (gameController.EnemyCount() == 0) {
            return;
        }
        /* Mouse Left Down (1 frame) */
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickedBullet() && State == BulletState.Waiting)
            {
                State = BulletState.Aiming;
                //print("mouse down");
            } else if (State == BulletState.Launching)
            {
                /* Use skill if bullet is being launched */
                playerBullet.GetComponent<BulletAmmo>().UseSkill();
            }
        }
        /* Mouse Left (press and hold) */
        else if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButtonUp(0))
        {
            /* If dragging, launches bullet */
            if (State == BulletState.Aiming)
            {
                Rigidbody2D rb = playerBullet.GetComponentInParent<Rigidbody2D>();
                rb.gravityScale = gravityScale;
                rb.AddForce(new Vector2((startingPoint.x - rb.position.x) * bulletForceMultiplier, (startingPoint.y - rb.position.y) * bulletForceMultiplier));
                State = BulletState.Launching;
                playerBullet.GetComponent<Animator>().SetBool("isLaunching", true);
            }

            if (State == BulletState.Waiting) { 
                LookAtPoint(new Vector3(startingPoint.x + 10f, startingPoint.y, 0));
            }
            //print("mouse up");
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("TestingScene");
        }
    }

    /* Returns current mouse position */
    Vector2 GetMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition;
    }

    /* If clicked on player bullet */
    bool ClickedBullet()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(GetMousePosition(), Vector3.zero, Mathf.Infinity, playerSelectedAmmo);

        if (hits.Length > 0)
        {
            return true;
        }
        return false;
    }

    /* Update good for physics apperently */
    float waitTime = 0f;
    void FixedUpdate()
    {
        gameObject.GetComponent<DebugController>().SetDebugText("Player State: " + State);
        if (State == BulletState.Aiming)
        {
            var position = Vector2.Lerp(playerBullet.position, GetMousePosition(), dragSmoothness * Time.deltaTime);
            var allowedPos = position - startingPoint;
            //playerBullet.position = new Vector2(Mathf.Clamp(position.x, startingPoint.x - mouseDragRadius, startingPoint.x + mouseDragRadius), Mathf.Clamp(position.y, startingPoint.y - mouseDragRadius, startingPoint.y + mouseDragRadius));
            playerBullet.position = startingPoint + Vector2.ClampMagnitude(allowedPos, mouseDragRadius);
            LookAtPoint(startingPoint);
            return;
        }
        if (State == BulletState.Collided) {
            //print(string.Format("Time.time: {0} | waitTime: {1} | resetWaitInSeconds: {2}", Time.time, waitTime, resetWaitInSeconds));
            /* Resets player to waiting after X seconds */
            if (Time.time >= waitTime + resetWaitInSeconds) {
                PostShot();
                State = BulletState.Waiting;
            }
        }
        /* Makes bullet rotate to direction of movement */
        else if (State == BulletState.Launching)
        {
            /* Stops launching and rotation if player collides */
            LookAtPoint(new Vector2(playerBullet.transform.position.x, playerBullet.transform.position.y) + playerBullet.gameObject.GetComponent<Rigidbody2D>().velocity);
        }
        return;
    }

    /* Sets the rotation of the transform */
    void LookAtPoint(Vector3 targetPoint)
    {
        Vector3 difference = targetPoint - playerBullet.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        playerBullet.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        //print(string.Format("Looking at point {0}. Diff = {1}", targetPoint, difference));
    }

    /* Switches ammo after shot */
    void PostShot()
    {
        if (State == BulletState.Collided)
        {
            Destroy(playerBullet.gameObject, 2f);
            bulletList.RemoveAt(0);
        }

        /* Moves bullet to starting position */
        if (GetCurrentBullet != null)
        {
            if (gameController.EnemyCount() > 0)
            {
                playerBullet = Instantiate(GetCurrentBullet.transform);
                playerBullet.GetComponent<ObjectPhysics>().playerController = this;

                playerBullet.position = startingPoint;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                LookAtPoint(new Vector3(startingPoint.x + 10f, startingPoint.y, 0));
            }
        }
    }

}
