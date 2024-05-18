using UnityEngine;

public class PawController : MonoBehaviour
{

    [SerializeField]
    private float hesitationPeriod = 0.5f;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Rigidbody2D pawPad;
    [SerializeField]
    private GameObject cat;
    [SerializeField]
    private AnimationCurve pawSpeed;
    [SerializeField]
    private float pawTravelTime = 0.5f;
    private float pawTravelDuration = 0;

    private float timer;
    private Vector2 targetPos;
    private Vector2 pawStartPos;
    [SerializeField]
    private float maxPawLength;
    private bool bap = false;
    Vector2 mousePos;
    private void Start()
    {
        timer = hesitationPeriod;
        float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        //maxPawLength = screenWidth * .4f;
        Debug.Log(screenWidth);
    }
    void Update()
    {
        timer -= Time.deltaTime;
       mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousePos.x);
        if (timer <= 0 && Vector2.Distance(mousePos, transform.position) < maxPawLength)
        {
            Debug.Log("bap");
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawStartPos = pawPad.position;
            timer = hesitationPeriod;
            bap = true;
        }
    }
    private void FixedUpdate()
    {
        if (bap)
        {
            if (targetPos != mousePos) targetPos = mousePos;
            Vector2 direction = (targetPos - (Vector2)cat.transform.position).normalized;
            pawTravelDuration += Time.fixedDeltaTime;
            float pawTravelProgress = Mathf.Clamp01(pawTravelDuration / pawTravelTime);
            pawPad.transform.position = (Vector2)cat.transform.position + direction * pawSpeed.Evaluate(pawTravelProgress) * Mathf.Min(maxPawLength, Vector2.Distance(cat.transform.position, targetPos));
            if (pawTravelProgress == 1)
            {
                pawTravelDuration = 0;
                bap = false;
            }
        }
        else
        {
            pawPad.position = cat.transform.position;
        }
    }
}
