using UnityEngine;

public class Movemint : MonoBehaviour
{
    [SerializeField]
    private float groundAccel = 200f;
    [SerializeField]
    private float airAccel = 200f;
    [SerializeField]
    private float maxGroundSpeed = 6.4f;
    [SerializeField]
    private float maxAirSpeed = 0.6f;
    [SerializeField]
    private float groundDecel = 8f;
    [SerializeField]
    private float jumpStrength = 5f;
    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private GameObject cameraObj;

    private float lastJumpTime = -1f;
    private float jumpInputDuration = 0.1f;
    private bool isGrounded = false;

    private void Update()
    {
        print(new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z).magnitude);
        if (Input.GetButton("Jump"))
        {
            lastJumpTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;
        playerVelocity = ApplyGroundDeceleration(playerVelocity);
        playerVelocity += ComputePlayerInput(input, playerVelocity);
        GetComponent<Rigidbody>().velocity = playerVelocity;
    }

    private Vector3 ApplyGroundDeceleration(Vector3 currentVelocity)
    {
        isGrounded = GroundCheck();
        float speed = currentVelocity.magnitude;

        if (!isGrounded || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        float reduction = speed * groundDecel * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - reduction, 0f) / speed);
    }

    private Vector3 ComputePlayerInput(Vector2 input, Vector3 velocity)
    {
        isGrounded = GroundCheck();

        float currentAccel = isGrounded ? groundAccel : airAccel;
        float currentMaxSpeed = isGrounded ? maxGroundSpeed : maxAirSpeed;

        Vector3 cameraRotation = new Vector3(0f, cameraObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(cameraRotation) *
                                new Vector3(input.x * currentAccel, 0f, input.y * currentAccel);

        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        Vector3 currentHorizVelocity = new Vector3(velocity.x, 0f, velocity.z);

        float maxSpeedRatio = Mathf.Max(0f, 1 - (currentHorizVelocity.magnitude / currentMaxSpeed));

        float velocityInputDot = Vector3.Dot(currentHorizVelocity, alignedInputVelocity);

        Vector3 adjustedVelocity = alignedInputVelocity * maxSpeedRatio;

        Vector3 finalVelocity = Vector3.Lerp(alignedInputVelocity, adjustedVelocity, velocityInputDot);

        finalVelocity += CalculateJumpVelocity(velocity.y);

        return finalVelocity;
    }

    private Vector3 CalculateJumpVelocity(float yVelocity)
    {
        Vector3 jumpVelocity = Vector3.zero;

        if (Time.time < lastJumpTime + jumpInputDuration && yVelocity < jumpStrength && GroundCheck())
        {
            lastJumpTime = -1f;
            jumpVelocity = new Vector3(0f, jumpStrength - yVelocity, 0f);
        }

        return jumpVelocity;
    }
    private bool GroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        bool result = Physics.Raycast(ray, GetComponent<Collider>().bounds.extents.y + 0.1f, groundLayerMask);
        return result;
    }
}
