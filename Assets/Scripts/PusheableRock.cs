using Mati36.Vinyl;
using UnityEngine;

public class PusheableRock : MonoBehaviour, IMovable
{
    public Rigidbody rb;
    public new BoxCollider collider;
    public bool isOnFloor;
    public bool isOnFloorPrevState;
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
    public VinylAsset impactSound;

    private Collider[] results;
    private RaycastHit[] hitResult = new RaycastHit[1];
    private Vector3 movementVector = Vector3.forward;

    private float secondsLimitInSamePlace = 0.15f;
    public float timerInSamePlace;

    private VinylAudioSource moveSound;

    public Vector3 pruebaCollider;

    private float secondsLimitToPlayImpactSound = 0.2f;
    public float timerInlimitToPlayImpactSound;

    private float timerToReproduceSound = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        timerInlimitToPlayImpactSound = secondsLimitInSamePlace;
        results = new Collider[1];
        timerInSamePlace = secondsLimitInSamePlace;
    }

    // Update is called once per frame
    void Update()
    {
        timerInSamePlace -= Time.deltaTime;
        CheckMovableObject();
        CheckFloor();
        SetKinematic();
        ApplyDrag();
        PlayRockMovingSound();

        timerToReproduceSound -= Time.deltaTime;
    }

    public void Execute(Vector3 direction)
    {
        OneDirectionMovement(direction);
    }

    private void PlayRockMovingSound()
    {
        if (timerToReproduceSound > 0) return;

        if (timerInSamePlace > 0 && isOnFloor)
        {
            if (!moveSound)
            {
                moveSound = rockMovingSound.PlayAt(transform.position);
                moveSound.Lock();
            }
            else if (!moveSound.IsPlaying)
                moveSound.RestartSource();
            else
                moveSound.transform.position = transform.position;
        }
        else
        {
            moveSound?.StopSource();
        }
    }

    private void OneDirectionMovement(Vector3 directionVector)
    {
        movementVector = directionVector.normalized;
        if (obstaclePos) return;
        timerInSamePlace = secondsLimitInSamePlace;
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
        isOnFloor = Physics.OverlapBox(floorDetectorTransform.position, pruebaCollider, Quaternion.identity, floorLayer).Length > 0;

        if (!isOnFloor)
            timerInlimitToPlayImpactSound -= Time.deltaTime;

        if (isOnFloorPrevState != isOnFloor && timerInlimitToPlayImpactSound <= 0)
        {
            isOnFloorPrevState = isOnFloor;
            impactSound.PlayAt(transform.position);
            timerInlimitToPlayImpactSound = secondsLimitToPlayImpactSound;
            FindObjectOfType<CameraBehaviour>().Shake(.4f, .35f, false);
        }
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
