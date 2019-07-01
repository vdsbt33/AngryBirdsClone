using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* Public variables */
    public Transform playerBullet;

    public Vector2 shootPositionOffset = new Vector2(0, 0);
    public float mouseDragRadius = 3f;
    public LayerMask playerSelectedAmmo;
    public float dragSmoothness = 1.0f;

    private bool isDragging = false;
    private Vector2 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = playerBullet.position;
    }

    /* Gets player click/hold */
    void Update()
    {
        /* Mouse Left Down (1 frame) */
        if (Input.GetMouseButtonDown(0))
        {
            if (ClickedBullet())
            {
                isDragging = true;
                print("mouse down");
            }
        }
        /* Mouse Left (press and hold) */
        else if (Input.GetMouseButton(0))
        {

        }
        if (Input.GetMouseButtonUp(0))
        {
            /* If dragging, launches bullet */
            //if (isDragging == true)
            //{
            //    Rigidbody2D rb = playerBullet.GetComponentInParent<Rigidbody2D>();
            //    rb.simulated = true;
            //    rb.AddForce(new Vector2(rb.position.x - startingPosition.x, rb.position.y - startingPosition.y));
            //}
            isDragging = false;
            LookAtPoint(new Vector3(startingPosition.x + 10f, startingPosition.y, 0));
            print("mouse up");
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
    void FixedUpdate()
    {
        if (isDragging)
        {
            var position = Vector2.Lerp(playerBullet.position, GetMousePosition(), dragSmoothness * Time.deltaTime);
            var allowedPos = position - startingPosition;
            //playerBullet.position = new Vector2(Mathf.Clamp(position.x, startingPosition.x - mouseDragRadius, startingPosition.x + mouseDragRadius), Mathf.Clamp(position.y, startingPosition.y - mouseDragRadius, startingPosition.y + mouseDragRadius));
            playerBullet.position = startingPosition + Vector2.ClampMagnitude(allowedPos, mouseDragRadius);
            LookAtPoint(startingPosition);
            return;
        }
        playerBullet.position = startingPosition;
        return;
    }

    /* Sets the rotation of the transform */
    void LookAtPoint(Vector3 targetPoint)
    {
        Vector3 difference = targetPoint - playerBullet.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        playerBullet.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        print(string.Format("Looking at point {0}. Diff = {1}", targetPoint, difference));
    }

}
