using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DriftTime : MonoBehaviour
{
    public PrometeoCarController car;

    public float driftAmount;
    public bool isDrifting;
    private float driftingTime;

    public TextMeshProUGUI driftAmountText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI scoreText;

    public GameObject playButton;
    public GameObject exitButton;
    public GameObject exitButton1;
    public GameObject restartButton;
    public GameObject aboutButton;

    private float ScoreMultiplier = 100f;
    private bool blinkActive;
    private float BlinkDuration = 3f;
    private float blinkTimer;
    private float BlinkInterval = 0.5f;

    private float countdownTimer = 100f;
    private bool isCountingDown = true;

    private int lap = 1;

    private bool lapTriggered = false;
    private float totalScore = 0f;
    private List<float> driftScores = new List<float>();
    private float gameDriftScore = 0f;

    private bool gameStarted = false;

    private Vector3 originalCarPosition;
    private Quaternion originalCarRotation;

    private void Start()
    {
        playButton.SetActive(true);
        exitButton.SetActive(true);
        aboutButton.SetActive(true);
        exitButton1.SetActive(false);
        restartButton.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        lapText.text = "Lap: " + lap;

        originalCarPosition = car.transform.position;
        originalCarRotation = car.transform.rotation;

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!gameStarted)
            return;

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

        if (car.transform.position.y < -10f)
        {
            RestartGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!gameStarted)
            return;

        if (other.CompareTag("LapTrigger") && !lapTriggered)
        {
            lapTriggered = true;
            lap++;
            lapText.text = "Lap: " + lap;
            driftScores.Add(gameDriftScore);
            gameDriftScore = 0f; 
        }
    }

    private void UpdateDriftAmountText()
    {
        if (driftAmountText != null)
        {
            driftAmountText.text = Mathf.RoundToInt(driftAmount).ToString();
            driftAmountText.gameObject.SetActive(true);
            StartBlinking();
            gameDriftScore = driftAmount;
        }
    }

    private void HideDriftAmountText()
    {
        if (driftAmountText != null)
        {
            driftAmountText.gameObject.SetActive(false);
        }
    }

    private void StartBlinking()
    {
        blinkActive = true;
        blinkTimer = BlinkDuration;
    }

    private void UpdateCountdownText()
    {
        if (countdownText != null)
        {
            countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
        }
    }

    public void PlayGame()
    {
        playButton.SetActive(false);
        exitButton.SetActive(false);
        restartButton.SetActive(false);
        aboutButton.SetActive(false);

        Time.timeScale = 1f;

        gameStarted = true;
    }

    private void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        Time.timeScale = 0f;
        lapText.gameObject.SetActive(false);

        driftScores.Add(gameDriftScore);

       
        foreach (float score in driftScores)
        {
            totalScore += score;
        }

        scoreText.gameObject.SetActive(true);
        int roundedScore = Mathf.RoundToInt(totalScore * lap);
        scoreText.text = "Score: " + roundedScore.ToString();

        
        restartButton.SetActive(true);
        exitButton1.SetActive(true);
    }

    public void RestartGame()
    {
        lap = 1;
        lapText.text = "Lap: " + lap;
        gameStarted = true;
        gameOverText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        lapText.gameObject.SetActive(true);
        Time.timeScale = 1f;
        totalScore = 0f;
        driftScores.Clear();
        gameDriftScore = 0f;

        car.transform.position = originalCarPosition;
        car.transform.rotation = originalCarRotation;

        countdownTimer = 100f;
        isCountingDown = true;

        driftAmount = 0f;
        driftingTime = 0f;
        gameDriftScore = 0f;

        restartButton.SetActive(false);
        exitButton1.SetActive(false);
    }

    public void OpenGitHubLink()
    {
        string githubURL = "https://github.com/maksimrudek/Drifting-Game";
        Application.OpenURL(githubURL);
    }

}
