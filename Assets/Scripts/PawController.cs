using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private float pawSpeedMultiplier;

    private float timer;
    private Vector2 targetPos;
    private Vector2 pawStartPos;
    private List<Vector2> posList = new List<Vector2>();
    private List<Vector2> lastRoute = new List<Vector2>();

    private GradingData currentGradingData;
    private GradingData nextGradingData;


    private float lowAngle;
    private float highAngle;
    private void Start()
    {
        timer = hesitationPeriod;
        pawStartPos = pawPad.position;
        nextGradingData = new GradingData(Camera.main.ScreenToWorldPoint(Input.mousePosition), new List<Vector2>(), Vector2.zero);
        currentGradingData = new GradingData(Camera.main.ScreenToWorldPoint(Input.mousePosition), new List<Vector2>(), Vector2.zero);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(nextGradingData.start, mousePos, Color.black);
        nextGradingData.positions.Add(mousePos);
        nextGradingData.end = mousePos;
        RenderGradingData(currentGradingData);
        RenderGradingData(nextGradingData);
        Debug.Log(Vector2.SignedAngle(new Vector2(0, 1), new Vector2(1, 0)));
        //Debug.Log(Vector2.Angle(pawPad.position.normalized, (pawPad.position.normalized + Vector2.up)));
        // Debug.Log(Vector2.Angle((pawPad.position + Vector2.up).normalized, ((Vector2)Input.mousePosition - pawPad.position).normalized));
        if (timer <= 0)
        {
            Debug.Log("bap");
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pawStartPos = pawPad.position;
            timer = hesitationPeriod;
            currentGradingData = nextGradingData;
            nextGradingData = new GradingData(targetPos, new List<Vector2>(), targetPos);
            posList.Clear();
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
    private void RenderGradingData(GradingData data)
    {
        Debug.DrawLine(data.start, data.end, Color.red);
        Vector2 lastPos = pawStartPos;
        foreach (Vector2 pos in data.positions)
        {
            Debug.DrawLine(lastPos, pos, Color.green);
            lastPos = pos;
        }
    }
}
