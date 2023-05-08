using UnityEngine;
using TMPro; // Add this namespace

public class TimerDisplay : MonoBehaviour
{
    public Timer timer;
    private TextMeshProUGUI timerText; 

    void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (timer.IsRunning())
        {
            float elapsedTime = timer.GetElapsedTime();
            int minutes = (int)elapsedTime / 60;
            int seconds = (int)elapsedTime % 60;
            int milliseconds = (int)(elapsedTime * 1000) % 1000;

            timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }
    }
}
