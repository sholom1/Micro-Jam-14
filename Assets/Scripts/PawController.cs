using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawController : MonoBehaviour
{
    [SerializeField]
    private AudioPlayer audioPlayer;

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
	private float extraHitDelay;
	[SerializeField]
    private float recordDistanceDelay;

    private float normalHitTimer = 0;
    private float extraHitTimer = 0;
    private float recordDistanceTimer = 0;

    private Vector2 targetPos;
    private Vector2 pawStartPos;

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
    }

    void Update()
    {
        normalHitTimer += Time.deltaTime;
        extraHitTimer += Time.deltaTime;

        recordDistanceTimer += Time.deltaTime;
        if (recordDistanceTimer >= recordDistanceDelay)
        {
			RecordMouseDistances();
            recordDistanceTimer = 0;
		}

        if (extraHitTimer >= extraHitDelay)
        {
            if (CheckIfExtraHit(out Vector3 position))
            {
                Bap(position);
            }
            extraHitTimer = 0;
        }

		if (normalHitTimer >= normalHitDelay)
        {
            Bap(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			normalHitTimer = 0;
        }
	}

	private void Bap(Vector3 position)
    {
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

    private bool CheckIfExtraHit(out Vector3 averagePosition)
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
}
