using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Interactable interactor;
    public float startAngle = 0f;
    private float currentAngle;
    public float angleToRotate = 45;

    [SerializeField] private GameObject _container;

    private RotableObject _rotableObject;

    // Start is called before the first frame update
    void Awake()
    {
        _rotableObject = new RotableObject(_container, Vector3.forward);
        currentAngle = startAngle;
        _container.transform.localRotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
        interactor.e_OnInteractionReceived += Action;
    }

    public void Action()
    {
        if (!_rotableObject.IsPlayingAnimation())
            TemporaryMoveHandle();
    }

    private void TemporaryMoveHandle()
    {
        StartCoroutine(_rotableObject.TemporaryAnimationForward(currentAngle, currentAngle + angleToRotate));
        currentAngle = currentAngle + angleToRotate;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_container.transform.position, Quaternion.AngleAxis(startAngle  -45, Vector3.up) * Vector3.forward );
    }
}
