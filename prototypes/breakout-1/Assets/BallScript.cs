using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using System; // For managing scenes

public class BallScript : MonoBehaviour
{
    [SerializeField] //gives us access to other components
    Rigidbody rb; //now a box will be created in inspector to drag and drop this into

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI levelText;
    public int score;
    public float timeRemaining = 180f; // 3 minutes (180 seconds)
    private bool isTimerRunning = false;
    private int level=1; // Variable to hold the level
    private int gameJustStarted = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.linearVelocity = new Vector3(4, -5, 0);
        isTimerRunning = true;

        // Get the saved game start state // Default to 1 if none saved
        gameJustStarted = PlayerPrefs.GetInt("gameJustStarted", 1);

        if (gameJustStarted == 1)
        {
            level = 1;
            gameJustStarted = 0;
            levelText.text = "Level: " + level;
            PlayerPrefs.SetInt("gameJustStarted", 0); 
        }
        else
        {
            level = PlayerPrefs.GetInt("Level", 1); 
            levelText.text = "Level: " + level;
        }
       
        

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -7f || transform.position.y > 7f || transform.position.x < -7f || transform.position.x > 7f || transform.position.z < -7f || transform.position.z > 7f)
        {
            // Reset the sphere's position to (0, 0, 0)
            transform.position = Vector3.zero;
        }
        scoreText.text = ("Score: ") + score.ToString();
        timerText.text = ("Time: ") + timeRemaining.ToString();
        if (isTimerRunning)
        {
            if (score >= 9)
            {
                // Save the current level in PlayerPrefs (to persist between scene reloads)
                PlayerPrefs.SetInt("Level", level + 1);
                PlayerPrefs.Save(); // Make sure changes are saved

                // Reload the current scene
                string sceneName = SceneManager.GetActiveScene().name; // Get the current scene's name
                SceneManager.LoadScene(sceneName); // Reload the current scene again
            }
            // Countdown timer logic
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Subtract the time elapsed since the last frame
                                                 // Convert time to minutes and seconds
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);

                // Display the time in the format MM:SS (e.g., 03:45)
                timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                rb.linearVelocity = new Vector3(0, 0, 0);
                timeRemaining = 0;
                isTimerRunning = false;
                timerText.text = ("Game Over!");
            }
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("brick")) //this will happen if thing hit is a brick 
        {
            Destroy(collision.gameObject);
            score++;
        }    
    }
}
