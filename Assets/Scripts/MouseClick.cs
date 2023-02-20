using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClick : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if left mouse button is clicked
        {
            Debug.Log("Click");
            Vector2 rayPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world position
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero); // Cast a ray from the mouse position

            if (hit.collider != null) // Check if the ray hits a collider
            {
                Debug.Log("Hit");
                GameObject clickedObject = hit.collider.gameObject; // Get the gameObject that the ray hits
                Debug.Log("Clicked on " + clickedObject.name); // Print the name of the clicked gameObject in the console
            }
        }
    }
}
