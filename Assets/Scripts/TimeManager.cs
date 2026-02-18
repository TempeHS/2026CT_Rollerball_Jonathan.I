using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText;

    private float currentTime = 0f;
    private bool timerRunning = true;

    private float bestTime = 0f;

    void Start()
    {
        // Load best time
        bestTime = PlayerPrefs.GetFloat("BestTime", 0f);

        if (bestTime > 0)
            bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
        else
            bestTimeText.text = "Best: ---";
    }

    void Update()
    {
        if (timerRunning)
        {
            currentTime += Time.deltaTime;
            timerText.text = "Time: " + currentTime.ToString("F2") + "s";
        }
    }

    public void StopTimer()
    {
        timerRunning = false;

        // Save best time if it's better
        if (bestTime == 0 || currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            bestTimeText.text = "Best: " + bestTime.ToString("F2") + "s";
        }
    }
}
