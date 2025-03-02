using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Player to follow
    public float defaultDistance = 3f; // Default distance from the player
    public float minDistance = 1f; // Minimum distance when colliding
    public float maxDistance = 4f; // Maximum distance when no obstacles
    public float mouseSensitivity = 2f;
    public float smoothSpeed = 5f; // Smooth transition speed
    public float collisionOffset = 0.2f; // Small offset to prevent clipping
    public LayerMask collisionMask; // Set in Inspector (Exclude player layer)
    public float followSpeed = 5f;     // How fast the camera follows position

    public Vector3 positionOffset;     // Offset for camera position relative to the player
    public Vector3 rotationOffset;     // Euler offset for camera rotation relative to the player
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 5f;

    private float rotationX = 0f, rotationY = 0f;
    private float currentDistance;

    void LateUpdate()
    {
        //// Update position
        //Vector3 desiredPosition = target.position + positionOffset;
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed * Time.deltaTime);

        //// Update rotation: combine player's rotation with an offset
        //Quaternion desiredRotation = target.rotation * Quaternion.Euler(rotationOffset);
        //transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentDistance = defaultDistance;
    }

    void Update()
    {
        //HandleRotation();
        HandleCollision();
    }

    void HandleRotation()
    {
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.position = target.position - (rotation * Vector3.forward * currentDistance);
        transform.LookAt(target.position);
    }

    void HandleCollision()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - target.position;
        float desiredDistance = defaultDistance;

        if (Physics.Raycast(target.position, direction.normalized, out hit, defaultDistance, collisionMask))
        {
            desiredDistance = Mathf.Clamp(hit.distance - collisionOffset, minDistance, maxDistance);
        }

        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * smoothSpeed);
    }
}
