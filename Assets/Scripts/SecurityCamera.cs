using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [SerializeField] private float maxRotationAngle = 45f;
    [SerializeField] private float rotationSpeed = 30f;

    [Header("Vision")]
    [SerializeField] private Transform visionOrigin;
    [SerializeField] private float viewDistance = 8f;

    [Range(0f, 180f)]
    [SerializeField] private float viewAngle = 60f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private Quaternion startRotation;
    private bool playerDetected;

    private void Start()
    {
        startRotation = transform.localRotation;
    }

    private void Update()
    {
        RotateCamera();
        CheckForPlayer();
    }

    private void RotateCamera()
    {
        float angle =
            Mathf.PingPong(
                Time.time * rotationSpeed,
                maxRotationAngle * 2f
            ) - maxRotationAngle;

        transform.localRotation =
            startRotation * Quaternion.Euler(0f, angle, 0f);
    }

    private void CheckForPlayer()
    {
        Collider[] players = Physics.OverlapSphere(
            visionOrigin.position,
            viewDistance,
            playerLayer
        );

        bool detectedThisFrame = false;

        foreach (Collider player in players)
        {
            Vector3 targetPosition = player.bounds.center;

            Vector3 directionToPlayer =
                targetPosition - visionOrigin.position;

            float distanceToPlayer = directionToPlayer.magnitude;

            Vector3 normalizedDirection =
                directionToPlayer.normalized;

            float angleToPlayer = Vector3.Angle(
                visionOrigin.forward,
                normalizedDirection
            );

            // Player ist outside the detection area
            if (angleToPlayer > viewAngle / 2f)
                continue;

            // Check if a Wall or other Obstacles could prevent the detection
            bool blocked = Physics.Raycast(
                visionOrigin.position,
                normalizedDirection,
                distanceToPlayer,
                obstacleLayer
            );

            if (!blocked)
            {
                detectedThisFrame = true;
                break;
            }
        }

        if (detectedThisFrame && !playerDetected)
        {
            playerDetected = true;
            Debug.Log("Player was detected by the Surveillance Camera!");
            rotationSpeed = 0f;
        }
        else if (!detectedThisFrame && playerDetected)
        {
            playerDetected = false;
            Debug.Log("Could not detect Player.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (visionOrigin == null)
            return;

        Gizmos.DrawWireSphere(
            visionOrigin.position,
            viewDistance
        );

        Vector3 leftBoundary =
            Quaternion.Euler(0f, -viewAngle / 2f, 0f)
            * visionOrigin.forward;

        Vector3 rightBoundary =
            Quaternion.Euler(0f, viewAngle / 2f, 0f)
            * visionOrigin.forward;

        Gizmos.DrawRay(
            visionOrigin.position,
            leftBoundary * viewDistance
        );

        Gizmos.DrawRay(
            visionOrigin.position,
            rightBoundary * viewDistance
        );
    }
}