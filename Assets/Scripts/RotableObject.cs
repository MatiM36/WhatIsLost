using System.Collections;
using UnityEngine;

public class RotableObject
{
    bool _isPlayingAnimation;
    private readonly Vector3 _axisToRotate;

    GameObject ObjectToRotate { get; }

    public RotableObject(GameObject objectToRotate, Vector3 axisToRotate)
    {
        ObjectToRotate = objectToRotate;
        _axisToRotate = axisToRotate;
    }

    public bool IsPlayingAnimation()
    {
        return _isPlayingAnimation;
    }

    public IEnumerator TemporaryAnimationForward(float initAngle, float endAngle)
    {
        _isPlayingAnimation = true;
        float interpolation = 0;

        while (ObjectToRotate.transform.rotation != Quaternion.AngleAxis(endAngle, _axisToRotate))
        {
            interpolation += Time.deltaTime;
            ObjectToRotate.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(initAngle, _axisToRotate), Quaternion.AngleAxis(endAngle, _axisToRotate), interpolation);
            yield return new WaitForEndOfFrame();
        }

        ObjectToRotate.transform.rotation = Quaternion.AngleAxis(endAngle, _axisToRotate);
        _isPlayingAnimation = false;
    }
}
