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
    [SerializeField]
    private PawController paw;
    [SerializeField]
    private float maxPawLengthScalarBeforeJump;
    [SerializeField]
    private float jumpForce;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseViewportPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float distanceBottomLeft = Vector2.Distance(new Vector2(0, 0), mouseViewportPosition);
        float distanceBottomRight = Vector2.Distance(new Vector2(1, 0), mouseViewportPosition);
        float distanceTopLeft = Vector2.Distance(new Vector2(0, 1), mouseViewportPosition);
        float distanceTopRight = Vector2.Distance(new Vector2(1, 1), mouseViewportPosition);

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceBetweenMouseAndCat = Vector2.Distance(transform.position, mouseWorldPosition);

        if (distanceBetweenMouseAndCat > paw.maxPawLength * maxPawLengthScalarBeforeJump)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                rigidbody.AddForce((mouseWorldPosition - (Vector2)transform.position).normalized * jumpForce);
                timer = hesitationTime;
            }
        }
        
        //Debug.Log(mousePos);
        //Debug.Log($"distanceBottomLeft: {distanceBottomLeft}");
        //Debug.Log($"distanceBottomRight: {distanceBottomRight}");
        //Debug.Log($"distanceTopLeft{distanceTopLeft}");
       // Debug.Log($"distanceTopRight{distanceTopRight}");
        //if (catState == CatState.Left && distanceBottomLeft > .4f)
        //{
        //    timer -= Time.deltaTime;
        //    if (timer <= 0)
        //    {
        //        rigidbody.AddForce(distanceTopLeft < .6f ? jumpForceUp : jumpForceRight);
        //        catState = CatState.Right;
        //        timer = hesitationTime;
        //    }
        //}
        //if (catState == CatState.Right && distanceBottomRight > .4f)
        //{
        //    timer -= Time.deltaTime;
        //    if (timer <= 0)
        //    {
        //        rigidbody.AddForce(distanceTopRight < .6f ? jumpForceUp : jumpForceLeft);
        //        catState = CatState.Left;
        //        timer = hesitationTime;
        //    }
        //}
    }
}
public enum CatState
{
    Left,
    Right,
    Jumping
}
