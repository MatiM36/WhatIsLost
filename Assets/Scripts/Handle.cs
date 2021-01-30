using System.Collections;
using UnityEngine;

public class Handle : MonoBehaviour, IActivator
{
    private const float START_ROTATION = 35;
    private const float END_ROTATION = -35;

    bool _isActivated;
    private RotableObject _rotableObject;

    [SerializeField] private GameObject handlePivot;
    [SerializeField] private Activatable activatable;


    // Start is called before the first frame update
    void Awake()
    {
        _rotableObject = new RotableObject(handlePivot, Vector3.right);
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
        {
            TemporaryMoveHandle();
            Execute();
        }
    }

    public void Execute()
    {
        activatable.Execute();
    }

    private void TemporaryMoveHandle()
    {
        if (_isActivated)
        {
            StartCoroutine(_rotableObject.TemporaryAnimationForward(END_ROTATION, START_ROTATION));
        }
        else
        {
            StartCoroutine(_rotableObject.TemporaryAnimationForward(START_ROTATION, END_ROTATION));
        }

        _isActivated = !_isActivated;
    }
}
