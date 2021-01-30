using System.Collections;
using UnityEngine;

public class Handle : MonoBehaviour, IActivator
{
    public float startRotation = 35;
    public float endRotation = -35;

    [SerializeField] private Transform handlePivot;
    [SerializeField] private Activatable activatable;

    bool _isActivated;
    private RotableObject _rotableObject;

    public Interactable interactor;

    void Awake()
    {
        _rotableObject = new RotableObject(handlePivot.gameObject, transform.right);
        interactor.e_OnInteractionReceived += Interact;
    }
    

    public void Interact()
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
            StartCoroutine(_rotableObject.TemporaryAnimationForward(endRotation, startRotation));
        }
        else
        {
            StartCoroutine(_rotableObject.TemporaryAnimationForward(startRotation, endRotation));
        }

        _isActivated = !_isActivated;
    }

    private void OnDrawGizmos()
    {
        if(handlePivot != null)
        {
            Gizmos.DrawRay(handlePivot.position, handlePivot.right);
        }
    }
}
