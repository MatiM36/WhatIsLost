using UnityEngine;

public class Activatable : MonoBehaviour, IActivatable
{
    public bool toggledOn = false;

    public virtual void Toggle(bool state)
    {
        toggledOn = state;
    }
}
