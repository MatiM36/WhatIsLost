using UnityEngine;

public class PusheableRock : MonoBehaviour
{
    public Rigidbody rb;
    public bool isOnFloor;
    public bool isTouchingPlayer;
    public Transform floorDetectorTransform;
    public float detectionRadius = 0.1f;
    public LayerMask floorLayer;

    public float floorDrag = 6f;
    public float airDrag = 0.05f;

    private Collider[] results;

    // Start is called before the first frame update
    void Start()
    {
        results = new Collider[1];
    }

    // Update is called once per frame
    void Update()
    {
        CheckFloor();
        SetKinematic();
        ApplyDrag();
    }

    private void OnCollisionStay(Collision collision)
    {
        isTouchingPlayer = false;

        var player = collision.gameObject.GetComponent<Player>();

        if (player)
            isTouchingPlayer = true;
    }

    private void SetKinematic()
    {
        if (isTouchingPlayer || !isOnFloor)
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

    private void OnDrawGizmos()
    {
        if (floorDetectorTransform != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(floorDetectorTransform.position, detectionRadius);
            if (isOnFloor)
                Gizmos.DrawWireCube(floorDetectorTransform.position, new Vector3(1, 0.2f, 1));
        }
    }
}
