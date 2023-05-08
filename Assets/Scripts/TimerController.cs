using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TimerController : MonoBehaviour
{
    public Transform player;
    public Transform endPlatform;
    public TextMeshProUGUI timerText;

    public string nextSceneName;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nextLevelPromptText;
    private float startTime;
    private float elapsedTime;
    private bool isRunning;
    private Vector3 spawnPosition;
    private bool isPlayerOnEndPlatform;
    private float bestScore;
    private List<Vector3> currentRunPositions;
    private List<Vector3> bestScorePositions;
    private int ghostPositionIndex;

    void Start()
    {
        spawnPosition = player.position;
    }

    void Update()
    {
        if (!isRunning && player.position.z > 0)
        {
            StartTimer();
        }
        if (isRunning && !isPlayerOnEndPlatform)
        {
            
            
        }
        if (isRunning)
        {
            isPlayerOnEndPlatform = IsPlayerOnPlatform(endPlatform);
            CheckForReset();
            if (isPlayerOnEndPlatform)
            {
                StopTimer();
                SaveScore();
                nextLevelPromptText.enabled = true;
                CheckForNextLevelInput();
            }
            else
            {
                UpdateTimerDisplay();
                nextLevelPromptText.enabled = false;
            }
        }

        if (player.position.y < -11)
        {
            player.position = spawnPosition;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StopTimer();
        }
    }

    private void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    private void StopTimer()
    {
        elapsedTime = Time.time - startTime;
        isRunning = false;
    }

    private void UpdateTimerDisplay()
    {
        float time = Time.time - startTime;
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int milliseconds = (int)(time * 1000) % 1000;

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    private bool IsPlayerOnPlatform(Transform platform)
    {
        float distance = Vector3.Distance(player.position, platform.position);
        float platformRadius = platform.GetComponent<Collider>().bounds.extents.magnitude;

        return distance <= platformRadius;
    }
    

   
   
    private void SaveScore()
    {
        float currentScore = elapsedTime;
        if (currentScore < bestScore)
        {
            bestScore = currentScore;


        }

        scoreText.text = timerText.text;
    }
   
    private void CheckForNextLevelInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void CheckForReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.position = spawnPosition;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene("Level 1");
        } 
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("Level 2");
        } 
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Level 3");
        }
        
    }

}

