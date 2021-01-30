using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject _objectToFollow;
    public float distanceToObject = 1;
    public float verticalOffset = 0.7f;
    public float smoothTime = 0.3f;
    public bool follow1To1;

    private float xDistance = -5;
    private float yDistance = 5;
    private float zDistance = -5;
    private Vector3 offsetPos;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        SearchPlayer();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        offsetPos = new Vector3(xDistance, yDistance, zDistance) * distanceToObject;

        if (_objectToFollow)
        {
            if (follow1To1)
                FollowObject1To1();
            else
                FollowObjectSmoothy();
        }
    }

    public void SetObject(GameObject objectToFollow)
    {
        _objectToFollow = objectToFollow;
        PositionCamera();
    }
    private void SearchPlayer()
    {
        _objectToFollow = FindObjectOfType<Player>().gameObject;
        PositionCamera();
        Debug.Log("La cámara está haciendo un FindObject del player. Al momento de spawnear hacer que el player sea el que hace el FindObject de la cámara y llame a la función SetObject");
    }

    private void PositionCamera()
    {
        offsetPos = new Vector3(xDistance, yDistance, zDistance);
        transform.position = _objectToFollow.transform.position + offsetPos;
        transform.LookAt(GetObjectPositionWithVerticalOffset());
    }

    private void FollowObject1To1()
    {
        transform.position = GetObjectPositionWithVerticalOffset() + offsetPos;
    }

    private void FollowObjectSmoothy()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetObjectPositionWithVerticalOffset() + offsetPos, ref velocity, smoothTime);
    }

    private Vector3 GetObjectPositionWithVerticalOffset()
    {
        return (_objectToFollow.transform.position + Vector3.up * verticalOffset);
    }
}
