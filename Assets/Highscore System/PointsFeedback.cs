using UnityEngine;
using TMPro;

public class PointsFeedback : MonoBehaviour
{
    [SerializeField]
    private float lifeSpan;
    private float timer;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    private void Start()
    {
        timer = lifeSpan;
    }
    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) Destroy(gameObject);
        nameText.canvasRenderer.SetAlpha(timer / lifeSpan);
        valueText.canvasRenderer.SetAlpha(timer / lifeSpan);
    }
}
