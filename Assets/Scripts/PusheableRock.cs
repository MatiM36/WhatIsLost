using UnityEngine;

public class PusheableRock : MonoBehaviour, IMovable
{
    public Rigidbody rb;
    public bool isOnFloor;
    public bool isTouchingPlayer;
    public Transform floorDetectorTransform;
    public float detectionRadius = 0.1f;
    public LayerMask floorLayer;

    public float floorDrag = 6f;
    public float airDrag = 0.05f;

    public float obstacleDetectionDistance = .9f;
    public Vector3 obstacleNormal;
    public bool movableObjDetected;
    public LayerMask movableLayer;
    public bool obstaclePos;

    private Collider[] results;
    private RaycastHit[] hitResult = new RaycastHit[1];
    private Vector3 movementVector = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        results = new Collider[1];
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovableObject();
        CheckFloor();
        SetKinematic();
        ApplyDrag();
    }

    private void OneDirectionMovement(Vector3 directionVector)
    {
        movementVector = directionVector.normalized;
        if (obstaclePos) return;
        transform.position += directionVector * Time.deltaTime;
    }

    public void Execute(Vector3 direction)
    {
        OneDirectionMovement(direction);
    }

    private void SetKinematic()
    {
        if (!isOnFloor)
            rb.isKinematic = false;
        else
            rb.isKinematic = true;
    }

    private void CheckFloor()
    {
        isOnFloor = Physics.OverlapSphereNonAlloc(floorDetectorTransform.position, detectionRadius, results, floorLayer) > 0;
    }

    private void ApplyDrag()
    {
        rb.drag = isOnFloor ? floorDrag : airDrag;
    }

    private void CheckMovableObject()
    {
        obstaclePos = Physics.RaycastNonAlloc(transform.position, movementVector, hitResult, obstacleDetectionDistance, movableLayer) > 0;
    }


    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = movableObjDetected ? Color.red : Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + movementVector * obstacleDetectionDistance);
        }

        if (floorDetectorTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(floorDetectorTransform.position, detectionRadius);
            if (isOnFloor)
                Gizmos.DrawWireCube(floorDetectorTransform.position, new Vector3(1, 0.2f, 1));
        }
    }
}
