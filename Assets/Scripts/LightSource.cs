using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    public LineRenderer line;
    public LayerMask lightLayer;

    public float maxLightTravelDistance = 100;
    public int maxBounces = 5;

    private int currentBounces = 0;

    private List<Vector3> points = new List<Vector3>();

    private RaycastHit[] hits;

    private void Awake()
    {
        hits = new RaycastHit[5];
    }

    private void Update()
    {
        points.Clear();
        points.Add(transform.position);
        Vector3 nextDir = Vector3.down;
        currentBounces = 0;
        bool nextPoint = false;
        do
        {
            var lastPoint = points[points.Count - 1];
            if (Physics.RaycastNonAlloc(lastPoint, nextDir, hits, maxLightTravelDistance, lightLayer) > 0)
            {
                for (int i = hits.Length - 1; i >= 0; i--)
                {
                    var hit = hits[i];
                    if (hit.collider == null) continue;

                    var reflector = hit.collider.gameObject.GetComponent<Reflector>();
                    var receiver = hit.collider.gameObject.GetComponent<LightReceiver>();

                    
                    if (reflector != null && (points.Count == 1 || Vector3.Dot(-reflector.forwardTransform.forward, nextDir) > -0.2)) //If it hits a reflector object from its reflecting face
                    {
                        points.Add(reflector.forwardTransform.position);
                        nextDir = reflector.forwardTransform.forward;
                        nextPoint = true;
                    }
                    else if (receiver != null)
                    {
                        points.Add(receiver.lightCenter.position);
                        nextPoint = false;
                        receiver.Toggle(true);
                    }
                    else //If it hits an obstacle
                    {
                        points.Add(hit.point);
                        nextPoint = false;
                    }

                    break;
                }
                
            }
            else
            {
                points.Add(lastPoint + nextDir * maxLightTravelDistance);
                nextPoint = false;
            }

            currentBounces++;

        } while (nextPoint && currentBounces < maxBounces);

        line.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            line.SetPosition(i, points[i]);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawWireSphere(points[i], 0.5f);
            Gizmos.DrawLine(points[i], points[i + 1]);
        }
    }
}
