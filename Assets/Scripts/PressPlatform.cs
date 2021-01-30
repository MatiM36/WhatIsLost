using UnityEngine;

public class PressPlatform : MonoBehaviour, IActivator
{
    private Vector3 _startPosition;

    public float pressedPosition = 1;

    public MeshRenderer mesh;

    public Activatable activatable;

    // Start is called before the first frame update
    void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 10)
        {
            mesh.transform.position = _startPosition - Vector3.down * pressedPosition;
            Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 10)
        {
            mesh.transform.position = _startPosition;
            Execute();
        }
    }

    public void Execute()
    {
        activatable.Execute();
    }
}
