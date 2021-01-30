using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public bool open = false;
    public Transform doorTransform;

    public float maxHeight = 0, minHeight = -1;
    private float t = 0;

    public float openTime = 2f;

    Vector3 closePos, openPos;

    private void Awake()
    {
        closePos = openPos = doorTransform.localPosition;
        closePos.z = maxHeight;
        openPos.z = minHeight;
    }

    public override void Toggle(bool state)
    {
        base.Toggle(state);

        open = state;
    }

    private void Update()
    {
        if(open && t < 1)
        {
            doorTransform.localPosition = Vector3.Lerp(closePos, openPos, t);
            t += Time.deltaTime / openTime;
        }
        else if(!open && t > 0)
        {
            doorTransform.localPosition = Vector3.Lerp(closePos, openPos,  t);
            t -= Time.deltaTime / openTime;
        }
    }
}
