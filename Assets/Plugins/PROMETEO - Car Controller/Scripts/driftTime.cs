using UnityEngine;
using TMPro;

public class driftTime : MonoBehaviour
{
    public PrometeoCarController car;

    public float driftAmount;
    public bool isDrifting;
    private float driftingTime;

    public TextMeshProUGUI driftAmountText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;

    private const float ScoreMultiplier = 100f;
    private bool blinkActive;
    private float BlinkDuration = 3f;
    private float blinkTimer;
    private float BlinkInterval = 0.5f;

    private float countdownTimer = 30f;
    private bool isCountingDown = true;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isCountingDown)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                countdownTimer = 0f;
                isCountingDown = false;
                GameOver();
            }

            UpdateCountdownText();
        }

        if (car.isDrifting)
        {
            driftingTime += Time.deltaTime;
            driftAmount = driftingTime * ScoreMultiplier;
            UpdateDriftAmountText();
        }
        else
        {
            driftingTime = 0f;
            driftAmount = 0f;
            HideDriftAmountText();

            if (blinkActive)
            {
                blinkTimer -= Time.deltaTime;

                if (blinkTimer <= 0f)
                {
                    blinkActive = false;
                    driftAmountText.gameObject.SetActive(false);
                }
                else
                {
                    float remainder = blinkTimer % BlinkInterval;
                    if (remainder <= BlinkInterval * 0.5f)
                    {
                        driftAmountText.gameObject.SetActive(true);
                    }
                    else
                    {
                        driftAmountText.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    void UpdateDriftAmountText()
    {
        if (driftAmountText != null)
        {
            driftAmountText.text = Mathf.RoundToInt(driftAmount).ToString();
            driftAmountText.gameObject.SetActive(true);
            StartBlinking();
        }
    }

    void HideDriftAmountText()
    {
        if (driftAmountText != null)
        {
            driftAmountText.gameObject.SetActive(false);
        }
    }

    void StartBlinking()
    {
        blinkActive = true;
        blinkTimer = BlinkDuration;
    }

    void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
        }
    }

    void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f; // Stop the game
    }
}
