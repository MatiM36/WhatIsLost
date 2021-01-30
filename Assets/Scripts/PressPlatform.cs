using System.Collections.Generic;
using UnityEngine;

public class PressPlatform : MonoBehaviour, IActivator
{
    private Vector3 _startPosition;

    public float pressedPosition = 1;

    public MeshRenderer mesh;

    public Activatable activatable;

    public LayerMask playerLayer;

    public HashSet<Collider> overlappingColliders = new HashSet<Collider>();

    // Start is called before the first frame update
    void Awake()
    {
        _startPosition = mesh.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            overlappingColliders.Add(other);
            
            mesh.transform.position = _startPosition - Vector3.down * pressedPosition;
            Execute();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            overlappingColliders.Remove(other);
            if (overlappingColliders.Count == 0)
            {
                mesh.transform.position = _startPosition;
                Execute();
            }
        }
    }

    public void Execute()
    {
        activatable.Execute();
    }
}
