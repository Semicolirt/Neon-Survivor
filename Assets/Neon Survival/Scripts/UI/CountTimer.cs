using TMPro;
using UnityEngine;

public class CountTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float countTime = 0f;

    private void Update()
    {
        countTime += Time.deltaTime;
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = FormatTime(countTime);
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
