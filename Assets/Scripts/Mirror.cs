using UnityEngine;

public class Mirror : MonoBehaviour
{
    private const float ROTATION_ANGLE = 45;

    [SerializeField] private GameObject _container;

    private RotableObject _rotableObject;

    // Start is called before the first frame update
    void Awake()
    {
        _rotableObject = new RotableObject(_container, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Action();
    }

    public void Action()
    {
        if (!_rotableObject.IsPlayingAnimation())
            TemporaryMoveHandle();
    }

    private void TemporaryMoveHandle()
    {
        float actualAngleRotation = _container.transform.eulerAngles.y;

        StartCoroutine(_rotableObject.TemporaryAnimationForward(actualAngleRotation, actualAngleRotation + ROTATION_ANGLE));
    }
}
