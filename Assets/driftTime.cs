using UnityEngine;
using TMPro;

public class driftTime : MonoBehaviour
{
    public PrometeoCarController car;

    public float driftAmount;
    public bool isDrifting;
    private float driftingTime;

    public TextMeshProUGUI driftAmountText; 

    private const float ScoreMultiplier = 100f;
    private bool blinkActive;
    private  float BlinkDuration = 2f;
    private float blinkTimer;
    private  float BlinkInterval = 0.5f; 

    void Update()
    {
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
}
