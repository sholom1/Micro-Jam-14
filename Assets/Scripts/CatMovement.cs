using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [SerializeField]
    private float hesitationTime = 2f;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private PawController paw;
    [SerializeField]
    private float maxPawLengthScalarBeforeJump;
    [SerializeField]
    private float jumpForce;

    private float timer;

    // Update is called once per frame
    void Update()
    {
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
    }
}
