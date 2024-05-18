using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawController : MonoBehaviour
{
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
    [SerializeField]
    private float maxDistance;

	[SerializeField]
	private float normalHitDelay;
	[SerializeField]
	private float bonusHitDelay;
	[SerializeField]
    private float recordDistanceDelay;

    private float normalHitTimer = 0;
    private float bonusHitTimer = 0;
    private float recordDistanceTimer = 0;

    private Vector2 targetPos;
    private Vector2 pawStartPos;

    private GradingData currentGradingData;
    private GradingData nextGradingData;

    Vector2 originalPosition;
    Vector2 previousPosition;
    Vector2 currentPosition;
	Vector2 previousDelta;
    Vector2 currentDelta;

    Vector2 positionsTotal;
    float distancesTotal;
    int distancesCount;

	private void Start()
    {
        pawStartPos = pawPad.position;
        nextGradingData = new GradingData(Camera.main.ScreenToWorldPoint(Input.mousePosition), new List<Vector2>(), Vector2.zero);
        currentGradingData = new GradingData(Camera.main.ScreenToWorldPoint(Input.mousePosition), new List<Vector2>(), Vector2.zero);
    }

    void Update()
    {
        normalHitTimer += Time.deltaTime;
        bonusHitTimer += Time.deltaTime;

        recordDistanceTimer += Time.deltaTime;
        if (recordDistanceTimer >= recordDistanceDelay)
        {
			RecordMouseDistances();
            recordDistanceTimer = 0;
		}

        if (bonusHitTimer >= bonusHitDelay)
        {
            if (CheckIfBonus(out Vector3 position))
            {
                //Debug.Log("bonus");
                Bap(position);
            }
            bonusHitTimer = 0;
        }

		if (normalHitTimer >= normalHitDelay)
        {
            Bap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            currentGradingData = nextGradingData;
            nextGradingData = new GradingData(targetPos, new List<Vector2>(), targetPos);

			normalHitTimer = 0;
        }

		RenderGrading();
	}

	private void Bap(Vector3 position)
    {
		//Debug.Log("bap");

		targetPos = position;
		pawStartPos = pawPad.position;
	}

	private void RecordMouseDistances()
    {
		currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		currentDelta = currentPosition - previousPosition;

		bool changedDirectionX = currentDelta.x * previousDelta.x < 0;
		bool changedDirectionY = currentDelta.y * previousDelta.y < 0;

		if (changedDirectionX || changedDirectionY)
        {
            positionsTotal += currentPosition;
			distancesTotal += Vector2.Distance(currentPosition, originalPosition);
            distancesCount++;

			originalPosition = currentPosition;
		}

		previousPosition = currentPosition;
		previousDelta = currentDelta;
	}

    private bool CheckIfBonus(out Vector3 averagePosition)
    {
		averagePosition = positionsTotal / distancesCount;

        float averageDistance = distancesTotal / distancesCount;
		float chance = Random.Range(0f, 1f) * maxDistance;

		positionsTotal = new Vector2();
		distancesTotal = 0;
		distancesCount = 0;

		if (chance < averageDistance)
        {
			return true;
		}
        return false;
    }

	private void FixedUpdate()
    {
        Vector2 direction = (targetPos - pawPad.position).normalized;
        float progress = Vector2.Distance(targetPos, pawPad.position) / Vector2.Distance(targetPos, pawStartPos);
        Vector2 nextPos = pawPad.position + direction * pawSpeed.Evaluate(progress) * pawSpeedMultiplier * Time.fixedDeltaTime;
        pawPad.MovePosition(nextPos);
        lineRenderer.SetPositions(new Vector3[] { cat.transform.position, nextPos });
    }

    private void RenderGrading()
    {
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Debug.DrawLine(nextGradingData.start, mousePos, Color.black);
		nextGradingData.positions.Add(mousePos);
		nextGradingData.end = mousePos;
		RenderGradingData(currentGradingData);
		RenderGradingData(nextGradingData);
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
