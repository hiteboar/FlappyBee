using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioController : MonoBehaviour
{
    public Obstacle prefab; // Prefab of the object to generate.
    public float generationInterval = 2; // Generation interval in seconds.

    public float initialSpeed = 2f; // Movement speed of the objects.
    public float maxSpeed = 3f;
    public float speedIncrement = 0.05f; // Speed increment per second


    public float initialFreeSpace = 6;
    public float minFreeSpace = 3;
    public float spaceReductionSpeed;
    public bool mixDifficulty = true;

    private float screenWidth;
    private float objectWidth = -1f;
    public float currentFreeSpace;
    public float currentSpeed;

    private bool endGame = false;

    public UnityEvent OnGameInit;

    void Start()
    {
        // Calculate the screen width in world coordinates.
        screenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;

        objectWidth = CalculateTotalObjectWidth();

        InitGame();
    }

    public void InitGame()
    {
        CancelInvoke(); // In case there is some Invokes running

        //Clear Childs
        foreach (Transform child in transform) {
            if (child.parent == transform) {
                Destroy(child.gameObject);
            }
        }

        currentFreeSpace = initialFreeSpace;
        currentSpeed = initialSpeed;
        endGame = false;
        Time.timeScale = 1f;

        // Start generating objects.
        InvokeRepeating("GenerateObject", 0f, generationInterval);


        OnGameInit?.Invoke();
    }

    void Update()
    {
        if (endGame) {
            WaitForPlayerTouch();
        }
        else {
            UpdateGame();
        }
    }

    void UpdateGame()
    {
        // Move all generated objects to the left.
        foreach (Transform child in transform) {
            if (child.parent == transform) {
                child.Translate(Vector3.left * currentSpeed * Time.deltaTime);

                // If the object is no longer visible, destroy it.
                if (child.position.x < -screenWidth - objectWidth) {
                    Destroy(child.gameObject);
                }
            }
        }

        if (currentFreeSpace > minFreeSpace) {
            currentFreeSpace -= spaceReductionSpeed * Time.deltaTime;
            if (currentFreeSpace < minFreeSpace) currentFreeSpace = minFreeSpace;
        }

        if (currentSpeed < maxSpeed) {
            currentSpeed += speedIncrement * Time.deltaTime;
            if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
        }
    }

    void WaitForPlayerTouch()
    {
        // Check for touch input on the screen.
        if (Input.touchCount > 0) {
            // Loop through all the touches detected.
            for (int i = 0; i < Input.touchCount; i++) {
                // Check if the current touch is a tap (phase began).
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    InitGame();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            InitGame();
        }
    }

    void GenerateObject(/*object sender, ElapsedEventArgs e*/)
    {
        Obstacle instance = Instantiate(prefab, transform);

        if (mixDifficulty) {
            instance.freeSpace = Random.Range(currentFreeSpace, initialFreeSpace);
        }
        else {
            instance.freeSpace = currentFreeSpace;
        }

        // Generate a new object at the appropriate position.
        Vector3 spawnPosition = new Vector3(screenWidth + objectWidth / 2, 0, 0);
        instance.transform.position = spawnPosition;
        instance.transform.rotation = Quaternion.identity;
    }

    float CalculateTotalObjectWidth()
    {
        float width = 0f;

        // Calculate the total width of all child objects.
        foreach (Transform child in prefab.transform) {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null) {
                width += renderer.bounds.size.x;
            }
        }

        return width;
    }

    public void EndGame()
    {
        endGame = true;
        Time.timeScale = 0f;
    }
}
