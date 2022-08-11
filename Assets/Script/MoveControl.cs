using UnityEngine;

public class MoveControl : MonoBehaviour
{
    // rigidbody object for adding velocity
    Rigidbody rb;

    // speed parameter for player
    [SerializeField] float speed = 65f;

    // vector for storing direction
    Vector3 direction;

    // More parameters for smooth movement
    float smooth_Time = 0.1f;
    float turnSmoothVelocity;

    // Main camera transform
    Transform cam;

    // To control the animations
    Animator animator;

    private void Awake()
    {
        // Making cursor invisible and locked for better gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    { 
        // Get rigidbody controller
        rb = GetComponent<Rigidbody>();

        // Get Camera transform
        cam = Camera.main.transform;

        // Get animator component
        animator = GetComponent<Animator>();
    }

    void Update()
    {        
        // Get values between -1 and 1 by using Arrow keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;       
    }

    private void FixedUpdate()
    {
        if(direction.magnitude <= 0.1f)
        {
            // Setting up animator parameters
            animator.SetBool("isWalking", false);
        }
        else if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("isWalking", true);

            // Get require angle by Atan2 and adding camera angle with it to make it control by camera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Smooth out the snap (angle change)
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smooth_Time);

            // Rotate player according to angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);    
         
            // Get world direction to rotate and move
            Vector3 MoveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            // Adding velocity by using MoveDir
            rb.velocity = (MoveDir.normalized * speed * Time.deltaTime);
        }          
    }
}