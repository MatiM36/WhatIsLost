using System.Collections;
using UnityEngine;

public class Handle : MonoBehaviour, IActivator
{
    bool _isActivated;
    bool _isPlayingAnimation;

    private Quaternion _startRotation;

    [SerializeField] private GameObject handlePivot;
    [SerializeField] private Activatable activatable;

    // Start is called before the first frame update
    void Awake()
    {
        _startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Action();
    }

    public void Action()
    {
        if (!_isPlayingAnimation)
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
            StartCoroutine(TemporaryAnimation(-1));
        }
        else
        {
            StartCoroutine(TemporaryAnimation(1));
        }

        _isActivated = !_isActivated;
    }

    IEnumerator TemporaryAnimation(float time)
    {
        _isPlayingAnimation = true;
        float interpolation = 0;

        while (handlePivot.transform.rotation != Quaternion.AngleAxis(-35 * time, Vector3.right))
        {
            interpolation += Time.deltaTime;
            handlePivot.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(35 * time, Vector3.right), Quaternion.AngleAxis(-35 * time, Vector3.right), interpolation);
            yield return new WaitForEndOfFrame();
        }

        _isPlayingAnimation = false;
    }
}
