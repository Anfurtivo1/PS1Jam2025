using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float moveSpeed;          // Movement speed
    public float rotateSpeed;        // Rotation speed (slower to prevent hyper-fast rotation)
    public float jumpForce;          // Jump force
    public float gravity;           // Gravity effect (can be adjusted for smoother fall)
    public Transform cameraTransform;     // Reference to the Camera's Transform

    private Rigidbody rb;                 // Rigidbody component
    private Vector3 moveDirection;        // Direction the player wants to move
    private bool isGrounded;              // Ground detection

    private float rotationY = 0f;         // Rotation angle on Y axis (for the player only)
    private float cameraRotationX = 0f;   // Camera's rotation angle on X axis (for pitch/vertical rotation)

    //private bool isRotating = false; // Whether the object is rotating
    private Quaternion targetRotation; // The target rotation to reach
    private Vector3 currentEuler;

    private CharacterController characterController;

    KeyCode pressedKey = KeyCode.None;
    private Vector3 velocity;           // Current velocity (including gravity)

    private bool isRotating = false;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        characterController = GetComponent<CharacterController>();

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // Prevent rotation
            rb.useGravity = true;  // Let physics handle gravity
        }
    }

    void Update()
    {
        // Handle movement and jumping
        HandleMovement();
        HandleJump();
        //HandleRotation();

        // Optional: Handle gravity manually
        ApplyGravity();
    }

    void FixedUpdate()
    {
        // Gravity should apply every fixed frame
        ApplyGravity();
    }

    // Handle movement based on input
    void HandleMovement()
    {

        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                pressedKey = KeyCode.W;

                currentEuler = transform.rotation.eulerAngles;
                currentEuler.y += 0f; // Suma 45 grados (usa -45f para restar)
                targetRotation = Quaternion.Euler(currentEuler);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                pressedKey = KeyCode.A;

                currentEuler = transform.rotation.eulerAngles;
                currentEuler.y += -90f; // Suma 45 grados (usa -45f para restar)
                targetRotation = Quaternion.Euler(currentEuler);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                pressedKey = KeyCode.S;

                currentEuler = transform.rotation.eulerAngles;
                currentEuler.y += 180f; // Suma 45 grados (usa -45f para restar)
                targetRotation = Quaternion.Euler(currentEuler);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                pressedKey = KeyCode.D;

                currentEuler = transform.rotation.eulerAngles;
                currentEuler.y += 90f; // Suma 45 grados (usa -45f para restar)
                targetRotation = Quaternion.Euler(currentEuler);
                isRotating = true;
            }
        }
        
        // Smoothly rotate toward the target rotation.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

        // Move the object in the direction it is facing while the key is held down.
        if (pressedKey != KeyCode.None && Input.GetKey(pressedKey))
        {
            Vector3 moveDirection = transform.forward * moveSpeed * Time.deltaTime;
            transform.position += moveDirection;
        }
        else
        {
            // Stop moving if the key is released.
            pressedKey = KeyCode.None; // Reset the pressed key
        }

        // Si la rotación actual está muy cerca de la objetivo, se considera finalizada.
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            transform.rotation = targetRotation;  // Asegura que se alinee perfectamente
            isRotating = false;                   // Permite procesar nuevos inputs
        }


    }

    // Handle rotation of the player and camera (turret-like control)
    //void HandleRotation()
    //{
    //    // Get mouse input for rotating (mouseX for the player rotation)
    //    float mouseX = Input.GetAxis("Mouse X"); // Horizontal mouse movement

    //    // Update rotationY based on mouseX input to rotate the player
    //    rotationY += mouseX * rotateSpeed;

    //    // Apply player rotation (player rotates independently on the Y-axis)
    //    transform.rotation = Quaternion.Euler(0f, rotationY, 0f); // Player's Y-axis rotation only

    //    // Adjust camera rotation for smooth movement (pitch/vertical movement)
    //    if (cameraTransform != null)
    //    {
    //        float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse movement (optional)

    //        cameraRotationX -= mouseY * rotateSpeed * Time.deltaTime; // Update vertical camera rotation
    //        cameraRotationX = Mathf.Clamp(cameraRotationX, -40f, 80f); // Limit the camera's vertical rotation

    //        // Apply the camera's vertical rotation (keeping the camera's rotation independent from player)
    //        cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    //    }
    //}

    // Apply gravity effect manually
   void ApplyGravity()
    {
        // Apply gravity
        if (characterController.isGrounded)
        {
            // Reset vertical velocity when grounded
            velocity.y = -2f; // A small value to make sure the character stays grounded
        }
        else
        {
            // Apply gravity if not grounded
            velocity.y += gravity * Time.deltaTime;
        }
    }
    // Jump logic

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //void Jump()
    //{
    //    // Reset the Y velocity before applying the jump force
    //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //}

    // Ground detection logic (simplified)
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
