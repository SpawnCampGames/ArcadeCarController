using UnityEngine;

public class CarController2 : MonoBehaviour
{
    public Rigidbody sphereRB;
    public Rigidbody carRB;

    public float maxFwdSpeed = 200f;
    public float fwdSpeed;
    
    public float fwdAccel;
    public float stoppingAccel;
    
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;

    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;
    
    private float normalDrag;
    public float modifiedDrag;
    
    public float alignToGroundTime;
    
    void Start()
    {
        // Detach Sphere from car
        sphereRB.transform.parent = null;
        carRB.transform.parent = null;

        normalDrag = sphereRB.drag;
    }
    
    void Update()
    {
        // Get Input
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
        
        // Added Acceleration to Forward Direction
        if (moveInput > 0) // if your forward input is greater than zero
        {
            if (fwdSpeed < maxFwdSpeed) // then we want to check if our fwdSpeed is less than our maxSpeed
            {
                fwdSpeed += Time.deltaTime * fwdAccel; // if it is then increase it with our acceleration as the multiplier
            }
            else // if its greater than our maxSpeed just set it to our maxSpeed
            {
                fwdSpeed = maxFwdSpeed;
            }
        }
        else // now if our forward input is less than or = to zero (we're not moving) so we want to decelerate instead
        {
            if (fwdSpeed > 0) // so we'll check if we do got forward speed
            {
                fwdSpeed -= Time.deltaTime * stoppingAccel; // and if we do we to subtract instead by our stopping acceleration
            }
        }

        // Calculate Turning Rotation
        float newRot = turnInput * turnSpeed * Time.deltaTime * moveInput;
        
        if (isCarGrounded)
            transform.Rotate(0, newRot, 0, Space.World);

        // Set Cars Position to Our Sphere
        transform.position = sphereRB.transform.position;

        // Raycast to the ground and get normal to align car with it.
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);
        
        //Rotate Car to align with ground
        Quaternion toRotateTo = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotateTo, alignToGroundTime * Time.deltaTime);
        
        // Calculate Movement Direction
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;
        
        //Calculate Drag
        sphereRB.drag = isCarGrounded ? normalDrag : modifiedDrag;
    }

    private void FixedUpdate()
    {
        if (isCarGrounded)
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration); // Add Movement
        else
            sphereRB.AddForce(transform.up * -200f); // Add Gravity
        
        carRB.MoveRotation(transform.rotation);
    }
}