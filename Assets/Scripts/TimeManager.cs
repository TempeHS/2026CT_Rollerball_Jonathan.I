using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestText;

    private float time;
    private bool running;

    private float bestTime;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // ONE-TIME RESET OF BEST TIME
        if (!PlayerPrefs.HasKey("BestTimeReset"))
        {
            PlayerPrefs.DeleteKey("BestTime");
            PlayerPrefs.SetInt("BestTimeReset", 1);
            PlayerPrefs.Save();
        }

        // Load best time
        bestTime = PlayerPrefs.GetFloat("BestTime", -1f);
        UpdateBestText();

        // Default timer display
        timerText.text = "Time: 00:00.00";
    }

    void Update()
    {
        if (!running) return;

        time += Time.deltaTime;
        timerText.text = "Time: " + Format(time);
    }

    public void Begin()
    {
        time = 0f;
        running = true;
    }

    public void Stop()
    {
        running = false;

        // Save new best time
        if (bestTime < 0f || time < bestTime)
        {
            bestTime = time;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
            UpdateBestText();
        }
    }

    void UpdateBestText()
    {
        if (bestTime < 0f)
            bestText.text = "Best: --:--.--";
        else
            bestText.text = "Best: " + Format(bestTime);
    }

    string Format(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t % 60f;
        return $"{minutes:00}:{seconds:00.00}";
    }
}
