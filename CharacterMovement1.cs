using NUnit.Framework.Internal.Commands;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement1 : MonoBehaviour
{

    [Header("Movement Options")]
    public float moveSpeed;
    public Rigidbody rb;
    public float jumpForce;
    public Vector3 CurrentVelocity;
    public float maxJumps;
    public float jumpCount;
    public float airControlMultiplier;
    public float fallSpeed = 1f;
    public float maxFallSpeed;
    public bool isGrounded;
    public bool inAir;
    public Transform pivot;
    public GameObject character;
    private float turnSpeed;

    [Header("Glide")]
    public float glideGravityMultiplier = 0.3f;
    public float glideFallSpeed = -3f;
    public bool isGliding;






    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        //Specebar to Jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            Jump();
        }

        //E to Glide
        if (Input.GetButton("Glide") && !isGrounded && rb.linearVelocity.y < 0)
        {
            isGliding = true;
        }

        else 
        {
            isGliding = false;
        }

    }

    void FixedUpdate()
    {
        Vector3 input = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 moveDir = pivot.TransformDirection(input);

        


            float control = inAir ? airControlMultiplier : 1f;

            Vector3 velocity = rb.linearVelocity;
            velocity.x = moveDir.x * moveSpeed * control;
            velocity.z = moveDir.z * moveSpeed * control;



        if (velocity.y < 0)
        {
            if (isGliding)
            {
                // Reduced gravity while gliding
                velocity += Physics.gravity.y * glideGravityMultiplier * Time.fixedDeltaTime * Vector3.up;

                // Cap glide fall speed
                if (velocity.y < glideFallSpeed)
                    velocity.y = glideFallSpeed;
            }

            else 
            {
                // Normal falling
                velocity += fallSpeed * Physics.gravity.y * Time.fixedDeltaTime * Vector3.up;

            }
        }



        rb.linearVelocity = velocity;

    }

    void Jump()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;

        jumpCount++;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            inAir = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        inAir = true;
    }




}
