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
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime;
            ObjectToRotate.transform.localRotation = Quaternion.Lerp(Quaternion.AngleAxis(initAngle, _axisToRotate), Quaternion.AngleAxis(endAngle, _axisToRotate), t);
            yield return null;
        }

        ObjectToRotate.transform.localRotation = Quaternion.AngleAxis(endAngle, _axisToRotate);
        _isPlayingAnimation = false;
    }
}
