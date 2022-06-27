using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    public Transform xA;
    public Transform xB;
    public Transform zA;
    public Transform zB;

    public Vector3 FinalOffset;
    public int Direction;
    public float StartYOffset;
    public float EndYOffset;
    public Collision ConnectedTo;

    void Awake()
    {
        Transform Angle = new GameObject().transform;

        Angle.LookAt(xA.position, zA.position);
        //Get the direction of the stairs
        if (xA.position.z > zA.position.z)
        {
            Direction = 2;
            if (ConnectedTo != null)
            {
                Vector3 center = ConnectedTo.m_BoxCollider.center;
                center.z -= 0.33f / 2;
                ConnectedTo.m_BoxCollider.center = center;

                Vector3 size = ConnectedTo.m_BoxCollider.size;
                size.z -= 0.33f;
                ConnectedTo.m_BoxCollider.size = size;
            }
                //ConnectedTo.FinalOffset.z = -0.33f;
        }
        else if (xA.position.z < zA.position.z)
        {
            Direction = 0;
            if (ConnectedTo != null)
            {
                Vector3 center = ConnectedTo.m_BoxCollider.center;
                center.z += 0.33f / 2;
                ConnectedTo.m_BoxCollider.center = center;

                Vector3 size = ConnectedTo.m_BoxCollider.size;
                size.z -= 0.33f;
                ConnectedTo.m_BoxCollider.size = size;
            }
                //ConnectedTo.FinalOffset.z = 0.33f;
        }
        else if (xA.position.x > zA.position.x)
        {
            Direction = 1;
            if (ConnectedTo != null)
            {
                Vector3 center = ConnectedTo.m_BoxCollider.center;
                center.x -= 0.33f / 2;
                ConnectedTo.m_BoxCollider.center = center;

                Vector3 size = ConnectedTo.m_BoxCollider.size;
                size.x -= 0.33f;
                ConnectedTo.m_BoxCollider.size = size;
            }
                //ConnectedTo.FinalOffset.x = -0.33f;
        }
        else if (xA.position.x < zA.position.x)
        {
            Direction = 3;
            if (ConnectedTo != null)
            {
                Vector3 center = ConnectedTo.m_BoxCollider.center;
                center.x += 0.33f / 2;
                ConnectedTo.m_BoxCollider.center = center;

                Vector3 size = ConnectedTo.m_BoxCollider.size;
                size.x -= 0.33f;
                ConnectedTo.m_BoxCollider.size = size;
            }
                //ConnectedTo.FinalOffset.x = 0.33f;
        }
    }

    public void GetMinMax(out Vector3 Min, out Vector3 Max)
    {
        Min = new Vector3();
        Max = new Vector3();

        Min.x = Mathf.Min(xA.position.x, Mathf.Min(xB.position.x, Mathf.Min(zA.position.x, zB.position.x))) + FinalOffset.x;
        Min.y = Mathf.Min(xA.position.y, Mathf.Min(xB.position.y, Mathf.Min(zA.position.y, zB.position.y))) + FinalOffset.y - StartYOffset;
        Min.z = -Mathf.Min(xA.position.z, Mathf.Min(xB.position.z, Mathf.Min(zA.position.z, zB.position.z))) + FinalOffset.z;

        Max.x = Mathf.Max(xA.position.x, Mathf.Max(xB.position.x, Mathf.Max(zA.position.x, zB.position.x))) + FinalOffset.x;
        Max.y = Mathf.Max(xA.position.y, Mathf.Max(xB.position.y, Mathf.Max(zA.position.y, zB.position.y))) + FinalOffset.y - EndYOffset;
        Max.z = -Mathf.Max(xA.position.z, Mathf.Max(xB.position.z, Mathf.Max(zA.position.z, zB.position.z))) + FinalOffset.z;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(xA.position, zA.position);
        Gizmos.DrawLine(xB.position, zB.position);
        Gizmos.DrawLine(xA.position, xB.position);
        Gizmos.DrawLine(zA.position, zB.position);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(xA.position, zA.position);
        Gizmos.DrawLine(xB.position, zB.position);
        Gizmos.DrawLine(xA.position, xB.position);
        Gizmos.DrawLine(zA.position, zB.position);
    }
}
