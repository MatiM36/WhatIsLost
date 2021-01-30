using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool isAlive = true;

    [Header("Refs")]
    public new Rigidbody rigidbody;
    public Animator animator;
    public PlayerView view;
    public Transform ragdoll;
    public Transform physicsCollider;
    public Hurtbox hurtbox;

    [Header("Movement")]
    public Vector3 horizontalAxis, verticalAxis;

    public float maxSpeed = 5;
    public float accel = 1;
    [Range(0, 1)]
    public float floorDrag = 0.05f;
    [Range(0, 1)]
    public float airDrag = 0.05f;

    private bool canMove = true;

    [Header("Floor Detection")]
    public Transform floorDetectorTransform;
    public float detectionRadius = 0.1f;
    public LayerMask floorLayer;

    [Header("Ledge Detection")]
    public Transform ledgeDetectorTransform;
    public Transform climbEndTransform;
    public float detectionLength = 1;
    public LayerMask ledgeLayer;

    [Header("Wall Detection")]
    public Transform wallDetectorTransform;
    public float wallDetectionDistance = 0.5f;
    public float handSeparation = 0.5f;
    public float handReach = 0.5f;
    public LayerMask wallLayer;
    public Vector3 obstaclePos;
    public Vector3 obstacleNormal;

    [Header("Move Objects")]
    public LayerMask movableLayer;

    [Header("Interaction")]
    public LayerMask interactionLayer;
    public Transform interactionTransform;
    public float interactionRadius = 0.5f;

    [Header("Jump")]
    public float jumpForce = 10f;
    private bool isJumping;

    private Vector3 lastDir = Vector3.forward;
    public Vector3 LastDir { get { return lastDir; } }

    [Header("Debug")]
    public bool isOnFloor;
    public bool ledgeDetected;
    public bool wallDetected;
    public bool movableObjDetected;
    public bool interactionDetected;

    private Collider[] results;
    private RaycastHit[] hitResult;

    private Vector3 ledgeStartPosition = Vector3.zero;
    private Vector3 ledgeEndPosition = Vector3.zero;

    private void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
        if (animator == null)
            animator = GetComponent<Animator>();

        results = new Collider[1];
        hitResult = new RaycastHit[1];

        hurtbox.e_OnHitReceived += Kill;
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        if (canMove && (hor != 0 || vert != 0))
        {
            if (rigidbody.velocity.magnitude < maxSpeed && !wallDetected)
            {
                rigidbody.velocity += horizontalAxis.normalized * hor * accel;
                rigidbody.velocity += verticalAxis.normalized * vert * accel;

            }


            lastDir = (horizontalAxis.normalized * hor + verticalAxis.normalized * vert).normalized;
            transform.forward = lastDir;

            animator.SetFloat("speed", rigidbody.velocity.magnitude);
            animator.SetBool("moveInput", true);
        }
        else
        {
            animator.SetBool("moveInput", false);
            animator.SetFloat("speed", 0f);
        }

        animator.SetFloat("velocityY", rigidbody.velocity.y);
       

        CheckFloor();
        CheckLedge();
        CheckWall();
        CheckInteraction();
        ApplyDrag();
    }

    private void CheckInteraction()
    {
        interactionDetected = Physics.OverlapSphereNonAlloc(interactionTransform.position, interactionRadius, results, interactionLayer) > 0;
        if (interactionDetected && Input.GetKeyDown(KeyCode.F))
        {
            var activator = results[0].GetComponent<IInteractuable>();
            if(activator != null)
            {
                activator.Interact();
            }
        }
    }

    private void CheckWall()
    {
        wallDetected = Physics.RaycastNonAlloc(wallDetectorTransform.position, lastDir, hitResult, wallDetectionDistance, wallLayer) > 0;
        animator.SetBool("wallDetected", wallDetected);
        if(wallDetected)
        {
            obstaclePos = hitResult[0].point;
            obstacleNormal = hitResult[0].normal.normalized;
            lastDir = -obstacleNormal;
            transform.forward = lastDir;
            movableObjDetected = false;
        }
        else
        {
            movableObjDetected = Physics.RaycastNonAlloc(wallDetectorTransform.position, lastDir, hitResult, wallDetectionDistance, movableLayer) > 0;

            if(movableObjDetected)
            {
                obstaclePos = hitResult[0].point;
                obstacleNormal = hitResult[0].normal.normalized;
                lastDir = -obstacleNormal;
                transform.forward = lastDir;
            }
        }
    }

    

    private void CheckLedge()
    {
        ledgeDetected = Physics.OverlapSphereNonAlloc(ledgeDetectorTransform.position, detectionLength, results, ledgeLayer) > 0;
        animator.SetBool("ledgeDetected", ledgeDetected);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
            canMove = false;
        }

    }


    private void CheckFloor()
    {
        isOnFloor = Physics.OverlapSphereNonAlloc(floorDetectorTransform.position, detectionRadius, results, floorLayer) > 0;
        animator.SetBool("isOnFloor", isOnFloor);
    }

    public void OnJumpStart()
    {
        rigidbody.velocity += Vector3.up * jumpForce;
        isJumping = true;
        canMove = false;

        animator.ResetTrigger("jump");
        Debug.Log("Jump Start");
    }

    public void OnJumpEnd()
    {
        canMove = true;
        isJumping = false;


        animator.ResetTrigger("jump");
        animator.ResetTrigger("climb");
        Debug.Log("Jump End");
    }

    public void OnClimbStart()
    {
        rigidbody.isKinematic = true;

        canMove = false;
        isJumping = false;
    }
    public void OnClimbEnd()
    {
        transform.position = climbEndTransform.position;
        rigidbody.isKinematic = false;

        animator.ResetTrigger("climb");
        canMove = true;
        Debug.Log("Climb End");
    }

    private void ApplyDrag()
    {
        var prevVel = rigidbody.velocity;
        prevVel *= 1 - floorDrag;
        prevVel.y = rigidbody.velocity.y;
        rigidbody.velocity = prevVel;
    }

    public void Kill()
    {
        if (!isAlive) return;
        ragdoll.gameObject.SetActive(true);
        ragdoll.SetParent(null, true);
        view.gameObject.SetActive(false);
        physicsCollider.gameObject.SetActive(false);
        rigidbody.isKinematic = true;
        enabled = false;
        isAlive = false;
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

        if (ledgeDetectorTransform != null)
        {
            Gizmos.color = ledgeDetected ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(ledgeDetectorTransform.position, detectionLength);
        }

        if(wallDetectorTransform != null)
        {
            Gizmos.color = wallDetected ? Color.red : Color.blue;
            Gizmos.DrawLine(wallDetectorTransform.position, wallDetectorTransform.position + lastDir * wallDetectionDistance);
            //Gizmos.DrawWireSphere(wallDetectorTransform.position + lastDir * wallDetectionDistance, wallDetectionRadius);
        }

        if(interactionTransform != null)
        {
            Gizmos.color = interactionDetected ? Color.magenta : Color.black;
            Gizmos.DrawWireCube(interactionTransform.position, Vector3.one * interactionRadius);
        }
    }
}
