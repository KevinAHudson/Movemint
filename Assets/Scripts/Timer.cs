using UnityEngine;

public class Timer : MonoBehaviour
{
    private float startTime;
    private float elapsedTime;
    private bool isRunning;

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = false;
    }

    public void StopTimer()
    {
        elapsedTime = Time.time - startTime;
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        if (isRunning)
        {
            return Time.time - startTime;
        }
        else
        {
            return elapsedTime;
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}
