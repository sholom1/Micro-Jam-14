using UnityEngine;

public class PawController : MonoBehaviour
{
    [SerializeField]
    private AudioPlayer audioPlayer;

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
    private float pawSpeedMultiplier;

    private float timer;
    private Vector2 targetPos;
    private Vector2 pawStartPos;

    private void Start()
    {
        timer = hesitationPeriod;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Debug.Log("bap");
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawStartPos = pawPad.position;
            timer = hesitationPeriod;
        }
    }
    private void FixedUpdate()
    {
        Vector2 direction = (targetPos - pawPad.position).normalized;
        float progress = Vector2.Distance(targetPos, pawPad.position) / Vector2.Distance(targetPos, pawStartPos);
        Vector2 nextPos = pawPad.position + direction * pawSpeed.Evaluate(progress) * pawSpeedMultiplier * Time.fixedDeltaTime;
        pawPad.MovePosition(nextPos);
        lineRenderer.SetPositions(new Vector3[] { cat.transform.position, nextPos });
    }
}
