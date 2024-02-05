using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject TopObj;
    public GameObject BottomObj;

    public float freeSpace;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D bottomBoxCollider = BottomObj.GetComponent<BoxCollider2D>();
        BoxCollider2D topBoxCollider = TopObj.GetComponent<BoxCollider2D>();

        // Get the reference to the main camera.
        mainCamera = Camera.main;

        float screenHeight = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        float lowestPosition = -screenHeight - (bottomBoxCollider.bounds.extents.y) + screenHeight * 0.05f; // the lowest position 10% of screenspace from the bottom 
        float highestPosition = screenHeight - (bottomBoxCollider.bounds.extents.y) - freeSpace - screenHeight * 0.05f;

        float bottomPosition = Random.Range(lowestPosition, highestPosition);


        BottomObj.transform.position = new Vector3(transform.position.x, bottomPosition, transform.position.z);
        TopObj.transform.position = new Vector3(transform.position.x, bottomPosition + bottomBoxCollider.bounds.extents.y + freeSpace + topBoxCollider.bounds.extents.y, transform.position.z);
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision != null) return;
    //}

}
