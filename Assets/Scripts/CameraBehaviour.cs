using Mati36.Vinyl;
using System.Collections;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject _objectToFollow;
    public float distanceToObject = 1;
    public float verticalOffset = 0.7f;
    public float smoothTime = 0.3f;
    public bool follow1To1;
    public VinylAsset shakeSound;
    public float shakeForce = 0.08f;
    public float duration = 5;

    private float xDistance = -5;
    private float yDistance = 5;
    private float zDistance = -5;
    private Vector3 offsetPos;
    private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        SearchPlayer();
        Shake(shakeForce, duration, true);
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
        transform.position = Vector3.SmoothDamp(transform.position, GetObjectPositionWithVerticalOffset() + offsetPos, ref velocity, smoothTime) + _shakeOffset;
    }

    private Vector3 GetObjectPositionWithVerticalOffset()
    {
        return (_objectToFollow.transform.position + Vector3.up * verticalOffset);
    }



    private Vector3 _shakeOffset = Vector3.zero;
    private float durationLowForce = 1.5f;

    public void Shake(float force, float duration, bool soundOn)
    {
        StartCoroutine(CalculateOffsetForShake(force, duration, Vector3.zero));
        if (soundOn)
            shakeSound.Play();
    }

    IEnumerator CalculateOffsetForShake(float force, float duration, Vector3 direction)
    {
        var timer = duration;
        var dir = direction;
        var lowforce = force / 3;
        var currentforce = 0.0f;

        while (timer > 0)
        {
            if (timer > duration / durationLowForce)
                currentforce = lowforce;
            else if (timer < duration / durationLowForce / 2)
                currentforce = lowforce;
            else currentforce = force;

            if (dir != Vector3.zero)
                _shakeOffset = dir * (Random.value * 2 - 1) * currentforce;
            else
                _shakeOffset = Random.insideUnitSphere * currentforce;

            timer -= Time.deltaTime;
            yield return null;
        }
        _shakeOffset = Vector3.zero;
    }
}
