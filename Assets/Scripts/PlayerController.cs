using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5.0f; // The force with which the object will jump.
    public float maxVelocity = 5.0f; // The force with which the object will jump.

    private Rigidbody2D rb; // Reference to the Rigidbody to apply jump force.
    private CircleCollider2D playerCollider; // Reference to the collider to know the sixe of the object.

    private Camera mainCamera;
    private float screenHeight;
    private float screenWidth;

    public UnityEvent OnPlayerCollides;
    public UnityEvent OnPlayerSucced;

    void Start()
    {
        // Get the reference to the Rigidbody.
        rb = GetComponent<Rigidbody2D>();

        playerCollider = rb.GetComponent<CircleCollider2D>();

        // Get the reference to the main camera.
        mainCamera = Camera.main;

        // Get the height and width of the screen in world coordinates.
        screenHeight = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y - playerCollider.radius;
        screenWidth = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - playerCollider.radius;
    }

    public void Init()
    {
        transform.position = Vector3.zero;
        if (rb != null)
            rb.velocity = Vector2.zero;
    }

    void Update()
    {
        // Check for touch input on the screen.
        if (Input.touchCount > 0) {
            // Loop through all the touches detected.
            for (int i = 0; i < Input.touchCount; i++) {
                // Check if the current touch is a tap (phase began).
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    Jump();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            Jump();
        }

        if (rb.velocity.y > maxVelocity) rb.velocity = new Vector2(rb.velocity.x, maxVelocity);
        else if (rb.velocity.y < -maxVelocity) rb.velocity = new Vector2(rb.velocity.x, -maxVelocity);
    }

    void FixedUpdate()
    {
        // Clamp player position to stay within the screen bounds.
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -screenWidth, screenWidth),
            Mathf.Clamp(transform.position.y, -screenHeight, screenHeight),
            transform.position.z
        );
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.gameObject.tag == "Trigger") {
            OnPlayerSucced?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag == "Obstacle") {
            OnPlayerCollides?.Invoke();
        }
    }
}
