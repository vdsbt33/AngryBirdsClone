using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /* Public variables */
    public Transform playerBullet;

    public Vector2 shootPositionOffset = new Vector2(0, 0);
    public float mouseDragRadius = 3f;
    public LayerMask playerSelectedAmmo;
    public float dragSmoothness = 1.0f;
    public float bulletForceMultiplier = 1.0f;
    public float gravityScale = 0.7f;
    public float resetWaitInSeconds = 2f;

    public GameController gameController;

    public enum BulletState
    {
        Waiting = 0,
        Aiming = 1,
        Launching = 2,
        Collided = 3,
        Ended
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
                playerBullet.position = startingPosition;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
                playerBullet.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0f;
                LookAtPoint(new Vector3(startingPosition.x + 10f, startingPosition.y, 0));
            } else if (value == BulletState.Collided)
            {
                waitTime = Time.time;
            }
            _state = value;
        }
    }

    private Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = playerBullet.position;
        playerBullet.GetComponent<ObjectPhysics>().playerController = this;
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
                rb.AddForce(new Vector2((startingPosition.x - rb.position.x) * bulletForceMultiplier, (startingPosition.y - rb.position.y) * bulletForceMultiplier));
                State = BulletState.Launching;
            }
            
            if (State == BulletState.Waiting) { 
                LookAtPoint(new Vector3(startingPosition.x + 10f, startingPosition.y, 0));
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
            var allowedPos = position - startingPosition;
            //playerBullet.position = new Vector2(Mathf.Clamp(position.x, startingPosition.x - mouseDragRadius, startingPosition.x + mouseDragRadius), Mathf.Clamp(position.y, startingPosition.y - mouseDragRadius, startingPosition.y + mouseDragRadius));
            playerBullet.position = startingPosition + Vector2.ClampMagnitude(allowedPos, mouseDragRadius);
            LookAtPoint(startingPosition);
            return;
        }
        if (State == BulletState.Collided) {
            //print(string.Format("Time.time: {0} | waitTime: {1} | resetWaitInSeconds: {2}", Time.time, waitTime, resetWaitInSeconds));
            /* Resets player to waiting after X seconds */
            if (Time.time == waitTime + resetWaitInSeconds) { 
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

}
