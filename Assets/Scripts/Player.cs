﻿using System;
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

    private Vector3 inputDir;

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
        animator.ResetTrigger("jump");
        canMove = true;
        Debug.Log("Climb End");
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
        PlayDieSound();
    }

    public void PlayStepSound()
    {
        view.PlayStepSound();
    }
    public void PlayJumpSound()
    {
        view.PlayJumpSound();
    }
    public void PlayDieSound()
    {
        view.PlayDieSound();
    }  
    
    public void PlayClimbSound()
    {
        view.PlayClimbSound();
    }

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

    private void Update()
    {
        if (interactionDetected && Input.GetKeyDown(KeyCode.F))
            Interact();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        inputDir = (horizontalAxis.normalized * hor + verticalAxis.normalized * vert).normalized;

        if (canMove && (hor != 0 || vert != 0))
        {
            bool changedDirection =  Vector3.Dot(lastDir, inputDir) < 0;
            if (rigidbody.velocity.magnitude < maxSpeed)
            {
                if (!wallDetected && !movableObjDetected) //Normal movement
                {
                    rigidbody.velocity += horizontalAxis.normalized * hor * accel;
                    rigidbody.velocity += verticalAxis.normalized * vert * accel;
                }
                else if(movableObjDetected ) //If pushing object
                {
                    if (changedDirection) //If changed direction, move normally
                    {
                        rigidbody.velocity += horizontalAxis.normalized * hor * accel;
                        rigidbody.velocity += verticalAxis.normalized * vert * accel;
                    }
                    else //If mantaining direction, keep last dir
                    {
                        rigidbody.velocity += lastDir * Mathf.Max(Mathf.Abs(hor), Mathf.Abs(vert)) * accel;
                    }
                }
            }

            if(!movableObjDetected || changedDirection)//Retains the last dir if and object is in front and we didn't change dir
                lastDir = inputDir;

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

        if (interactionDetected)
            view.ShowInteractUI();
        else
            view.HideInteractUI();
    }

    private void Interact()
    {
        var activator = results[0].GetComponent<IInteractuable>();

        if (activator != null)
            activator.Interact();
    }

    private void CheckWall()
    {
        wallDetected = Physics.RaycastNonAlloc(wallDetectorTransform.position, lastDir, hitResult, wallDetectionDistance, wallLayer) > 0;
        animator.SetBool("wallDetected", wallDetected);
        if (wallDetected)
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

            if (movableObjDetected)
            {
                var movableObject = hitResult[0].transform.gameObject.GetComponent<IMovable>();

                obstaclePos = hitResult[0].point;
                obstacleNormal = hitResult[0].normal.normalized;
                lastDir = -obstacleNormal;
                transform.forward = lastDir;

                if (movableObject != null)
                    movableObject.Execute(lastDir);
            }
        }
    }

    private void CheckLedge()
    {
        ledgeDetected = Physics.OverlapSphereNonAlloc(ledgeDetectorTransform.position, detectionLength, results, ledgeLayer) > 0;
        animator.SetBool("ledgeDetected", ledgeDetected);
    }

    private void Jump()
    {
        animator.SetTrigger("jump");
        canMove = false;
    }

    private void CheckFloor()
    {
        isOnFloor = Physics.OverlapSphereNonAlloc(floorDetectorTransform.position, detectionRadius, results, floorLayer) > 0;
        animator.SetBool("isOnFloor", isOnFloor);
    }    

    private void ApplyDrag()
    {
        var prevVel = rigidbody.velocity;
        prevVel *= 1 - floorDrag;
        prevVel.y = rigidbody.velocity.y;
        rigidbody.velocity = prevVel;
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

        if (wallDetectorTransform != null)
        {
            Gizmos.color = wallDetected ? Color.red : Color.blue;
            Gizmos.DrawLine(wallDetectorTransform.position, wallDetectorTransform.position + lastDir * wallDetectionDistance);
            //Gizmos.DrawWireSphere(wallDetectorTransform.position + lastDir * wallDetectionDistance, wallDetectionRadius);
        }

        if (interactionTransform != null)
        {
            Gizmos.color = interactionDetected ? Color.magenta : Color.black;
            Gizmos.DrawWireCube(interactionTransform.position, Vector3.one * interactionRadius);
        }
    }
}
