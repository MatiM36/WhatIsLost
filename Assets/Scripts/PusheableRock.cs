using Mati36.Vinyl;
using UnityEngine;

public class PusheableRock : MonoBehaviour, IMovable
{
    public Rigidbody rb;
    public new BoxCollider collider;
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

    public VinylAsset rockMovingSound;

    private Collider[] results;
    private RaycastHit[] hitResult = new RaycastHit[1];
    private Vector3 movementVector = Vector3.forward;

    private Vector3 currentPosition;
    private Vector3 prevPosition;

    private float secondsLimitInSamePlace = 0.15f;
    public float timerInSamePlace;

    private VinylAudioSource sound;

    public Vector3 pruebaCollider;

    // Start is called before the first frame update
    void Start()
    {
        results = new Collider[1];
        prevPosition = transform.position;
        timerInSamePlace = secondsLimitInSamePlace;
    }

    // Update is called once per frame
    void Update()
    {
        timerInSamePlace -= Time.deltaTime;
        currentPosition = transform.position;
        CheckMovableObject();
        CheckFloor();
        SetKinematic();
        ApplyDrag();
        PlayRockMovingSound();
        prevPosition = currentPosition;
    }

    public void Execute(Vector3 direction)
    {
        timerInSamePlace = secondsLimitInSamePlace;
        OneDirectionMovement(direction);
    }

    private void PlayRockMovingSound()
    {
        if (timerInSamePlace > 0 && isOnFloor)
        {
            if (!sound)
            {
                sound = rockMovingSound.PlayAt(transform.position);
                sound.Lock();
            }
            else if (!sound.IsPlaying)
                sound.RestartSource();
            else
                sound.transform.position = transform.position;
        }
        else
        {
            sound?.StopSource();
        }
    }

    private void OneDirectionMovement(Vector3 directionVector)
    {
        movementVector = directionVector.normalized;
        if (obstaclePos) return;
        rb.position += directionVector * Time.deltaTime;
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
        isOnFloor = Physics.OverlapBox(floorDetectorTransform.position, pruebaCollider, Quaternion.identity,floorLayer).Length > 0;
    }

    private void ApplyDrag()
    {
        rb.drag = isOnFloor ? floorDrag : airDrag;
    }

    private void CheckMovableObject()
    {
        var side = Vector3.Cross(movementVector, Vector3.up);

        obstaclePos = Physics.RaycastNonAlloc(transform.position + side * collider.extents.x, movementVector, hitResult, obstacleDetectionDistance, movableLayer) > 0;
        obstaclePos |= Physics.RaycastNonAlloc(transform.position - side * collider.extents.x, movementVector, hitResult, obstacleDetectionDistance, movableLayer) > 0;
        obstaclePos |= Physics.RaycastNonAlloc(transform.position, movementVector, hitResult, obstacleDetectionDistance, movableLayer) > 0;
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = movableObjDetected ? Color.red : Color.blue;

            var side = Vector3.Cross(movementVector, Vector3.up);
            Gizmos.DrawRay(transform.position + side * collider.extents.x, movementVector * obstacleDetectionDistance);
            Gizmos.DrawRay(transform.position - side * collider.extents.x, movementVector * obstacleDetectionDistance);
        }

        if (floorDetectorTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(floorDetectorTransform.position, detectionRadius);
            if (isOnFloor)
                Gizmos.DrawWireCube(floorDetectorTransform.position, pruebaCollider);
        }
    }
}
