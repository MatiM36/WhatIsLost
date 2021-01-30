using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Refs")]
    public new Rigidbody rigidbody;

    [Header("Movement")]
    public Vector3 horizontalAxis, verticalAxis;

    public float maxSpeed = 5;
    public float accel = 1;
    [Range(0,1)]
    public float floorDrag = 0.05f;
    [Range(0,1)]
    public float airDrag = 0.05f;

    [Header("Floor Detection")]
    public Transform floorDetectorTransform;
    public float detectionRadius = 0.1f;
    public LayerMask floorLayer;

    [Header("Debug")]
    private Vector3 lastDir;
    
    public bool isOnFloor;

    private Collider[] results;

    private void Awake()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

        results = new Collider[1];
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var vert = Input.GetAxis("Vertical");
        if (hor != 0 || vert != 0)
        {
            if (rigidbody.velocity.magnitude < maxSpeed)
            {
                rigidbody.velocity += horizontalAxis.normalized * hor * accel;
                rigidbody.velocity += verticalAxis.normalized * vert * accel;
            }
            lastDir = (horizontalAxis.normalized * hor + verticalAxis.normalized * vert).normalized;
            transform.forward = lastDir;
        }

        CheckFloor();
        ApplyDrag();
    }

    private void CheckFloor()
    {
        isOnFloor = Physics.OverlapSphereNonAlloc(floorDetectorTransform.position, detectionRadius, results, floorLayer) > 0;
    }

    private void ApplyDrag()
    {
        rigidbody.velocity *= 1 - floorDrag;
    }

    private void OnDrawGizmos()
    {
        if (floorDetectorTransform == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(floorDetectorTransform.position, detectionRadius);
        if (isOnFloor)
            Gizmos.DrawWireCube(floorDetectorTransform.position, new Vector3(1, 0.2f, 1));
    }
}
