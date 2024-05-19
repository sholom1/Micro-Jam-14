using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float TimerDuration;
    private float timer;
    bool timerElapsed = false;
    [SerializeField]
    private UnityEvent onTimerElapsed;
    [SerializeField]
    private Image timerDisplay;

    private void Start()
    {
        timer = TimerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerElapsed) return;
        timer -= Time.deltaTime;
        timerDisplay.fillAmount = timer / TimerDuration;
        if (timer <= 0)
        {
            timerElapsed = true;
            onTimerElapsed.Invoke();
        }
    }
}
