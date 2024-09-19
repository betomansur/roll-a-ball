using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

   [SerializeField] private string MainMenu;

 // Rigidbody of the player.
 private Rigidbody rb; 

 // Variable to keep track of collected "PickUp" objects.
 private int count;

 // Movement along X and Y axes.
 private float movementX;
 private float movementY;

 // Speed at which the player moves.
 public float speed = 0;

 // UI text component to display count of "PickUp" objects collected.
 public TextMeshProUGUI countText;

public TextMeshProUGUI timerText;


 // UI object to display winning text.
 public GameObject winTextObject;

private float timer;

private bool hasWon = false; // To track if the player has won



 // Start is called before the first frame update.
 void Start()
    {
 // Get and store the Rigidbody component attached to the player.
        rb = GetComponent<Rigidbody>();

 // Initialize count to zero.
        count = 0;

 // Update the count display.
        SetCountText();

         timer = 0;


 // Initially set the win text to be inactive.
        winTextObject.SetActive(false);
    }
 
 // This function is called when a move input is detected.
 void OnMove(InputValue movementValue)
    {
 // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

 // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

 // FixedUpdate is called once per fixed frame-rate frame.
 private void FixedUpdate() 
    {
 // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

 // Apply force to the Rigidbody to move the player.
        rb.AddForce(movement * speed); 

        Die();

  if (!hasWon) // Only update the timer if the player hasn't won yet
        {
            // Update the timer every frame
            timer += Time.deltaTime;

            // Update the timer text on the UI
            timerText.text = "Time: " + timer.ToString("F2") + " s"; // Shows time in 2 decimal places
        }
    
    }

 
 void OnTriggerEnter(Collider other) 
    {
 // Check if the object the player collided with has the "PickUp" tag.
 if (other.gameObject.CompareTag("PickUp")) 
        {
 // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);

 // Increment the count of "PickUp" objects collected.
            count = count + 1;

 // Update the count display.
            SetCountText();
        }
    }

void Die()
    {
        // Check if player falls below Y position of -10
        if (transform.position.y < -10)
        {
               Debug.Log("Player died - Returning to Main Menu");

            SceneManager.LoadScene(MainMenu); // Return to the main menu
        }
    }
 // Function to update the displayed count of "PickUp" objects collected.
 void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            winTextObject.SetActive(true);
                    Debug.Log("Win");
                        hasWon = true; // Stop the timer since the player won


            StartCoroutine(WinAndReturnToMenu()); // Inicia a Coroutine para aguardar 3 segundos
        }
    }

    // Coroutine para aguardar 3 segundos e depois voltar ao menu
    IEnumerator WinAndReturnToMenu()
    {
        yield return new WaitForSeconds(3); // Espera por 3 segundos
        SceneManager.LoadScene(MainMenu); 
    }
}