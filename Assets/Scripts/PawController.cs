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
    private float pawTravelTime = 0.5f;
    private float pawTravelDuration = 0;

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

    Vector2 originalPosition;
    Vector2 previousPosition;
    Vector2 currentPosition;
	Vector2 previousDelta;
    Vector2 currentDelta;

    Vector2 positionsTotal;
    float distancesTotal;
    int distancesCount;

	private void Start()
    public float maxPawLength;
    private bool bap = false;
    Vector2 mousePos;
    private void Start()
    {
        timer = hesitationPeriod;
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
        if (Vector2.Distance(transform.position, position) < maxPawLength)
        {
            targetPos = position;
            pawStartPos = pawPad.position;

            audioPlayer.PlayNormalHitSFX();
        }
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
        lineRenderer.SetPositions(new Vector3[] { cat.transform.position, pawPad.transform.position });
    }
}
