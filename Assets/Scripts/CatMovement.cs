using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [SerializeField]
    private float hesitationTime = 2f;
    [SerializeField]
    private Vector2 jumpForceLeft;
    [SerializeField]
    private Vector2 jumpForceRight;
    [SerializeField]
    private Vector2 jumpForceUp;
    [SerializeField]
    private CatState catState = CatState.Left;
    [SerializeField]
    private Rigidbody2D rigidbody;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float distanceBottomLeft = Vector2.Distance(new Vector2(0, 0), mousePos);
        float distanceBottomRight = Vector2.Distance(new Vector2(1, 0), mousePos);
        float distanceTopLeft = Vector2.Distance(new Vector2(0, 1), mousePos);
        float distanceTopRight = Vector2.Distance(new Vector2(1, 1), mousePos);
        //Debug.Log(mousePos);
        //Debug.Log($"distanceBottomLeft: {distanceBottomLeft}");
        //Debug.Log($"distanceBottomRight: {distanceBottomRight}");
        //Debug.Log($"distanceTopLeft{distanceTopLeft}");
       // Debug.Log($"distanceTopRight{distanceTopRight}");
        if (catState == CatState.Left && distanceBottomLeft > .4f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                rigidbody.AddForce(distanceTopLeft < .6f ? jumpForceUp : jumpForceRight);
                catState = CatState.Right;
                timer = hesitationTime;
            }
        }
        if (catState == CatState.Right && distanceBottomRight > .4f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                rigidbody.AddForce(distanceTopRight < .6f ? jumpForceUp : jumpForceLeft);
                catState = CatState.Left;
                timer = hesitationTime;
            }
        }
    }
}
public enum CatState
{
    Left,
    Right,
    Jumping
}
