using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour
{
    [SerializeField] float slowTimeDuration = 5.0f;
    [SerializeField] float slowTimeCooldown = 10.0f;
    public float slowTimeScale = 0.5f;
    float timerValue;
    bool isSlowed;
    bool canSlow = true;
    float fillFraction;
    void Start()
    {
        timerValue = slowTimeDuration;
        fillFraction = timerValue / slowTimeDuration;
    }
    void Update()
    {
        SlowTime();
        Debug.Log(Time.timeScale);
        Debug.Log(timerValue);
    }

    void SlowTime()
    {

        isSlowed = Input.GetKey(KeyCode.V);
        if (isSlowed && canSlow)
        {
            timerValue -= Time.deltaTime;
            if (timerValue > 0)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                canSlow = false;
                StartCoroutine(CanSlow());
            }
        }
        else
        {
            Time.timeScale = 1.0f;
        }

    }

    IEnumerator CanSlow()
    {
        canSlow = false;
        Debug.Log("Can't slow");
        yield return new WaitForSeconds(slowTimeCooldown);
        Debug.Log("Can slow");
        timerValue = slowTimeDuration;
        canSlow = true;
    }
}
