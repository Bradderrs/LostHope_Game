 using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformMove : MonoBehaviour
{

    private Transform currentLocation;
    public Transform[] points;
    public float travelSpeed;
    private int nextPos = 0;
    private float distance;
    private bool movingForward;


    void Start()
    {
        currentLocation = transform;

        if (points != null && points.Length > 0)
        {
            transform.position = points[0].position;
            nextPos = 0;
            movingForward = true;
        }
       
    }

    void FixedUpdate()
    {
        

        transform.position = Vector3.MoveTowards(currentLocation.position, points[nextPos].position, travelSpeed * Time.deltaTime);
        distance = Vector3.Distance(currentLocation.position, points[nextPos].position);

        if (distance < 0.1f)
        {
            AdvanceTarget();
        }

        if (nextPos >= points.Length - 1)
        {
            movingForward = false;
        }

        if (nextPos == 0)
        {
            movingForward = true;
        }

    }
    void AdvanceTarget()
    {
        if (movingForward)
        {
            nextPos++;
        }

        if (!movingForward)
        {
            nextPos--;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }


    void OnDrawGizmos()
    {
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(currentLocation.position, points[nextPos].position);
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }
    }

    
}
