using UnityEngine;

public class Stairs : MonoBehaviour
{
    private Transform xA;
    private Transform xB;
    private Transform zA;
    private Transform zB;

    [HideInInspector]
    public int Direction;
    public float StartYOffset;
    public float EndYOffset;
    [Header("Put the collisions where the stairs is ending (top of the stairs)\nif the collider is blocking the player")]
    public Collision ConnectedTo;

    void Awake()
    {
        GetGameObjects();

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
        }
    }

    public void GetMinMax(out Vector3 Min, out Vector3 Max)
    {
        Min = new Vector3();
        Max = new Vector3();

        Min.x = Mathf.Min(xA.position.x, Mathf.Min(xB.position.x, Mathf.Min(zA.position.x, zB.position.x)));
        Min.y = Mathf.Min(xA.position.y, Mathf.Min(xB.position.y, Mathf.Min(zA.position.y, zB.position.y))) - StartYOffset;
        Min.z = -Mathf.Min(xA.position.z, Mathf.Min(xB.position.z, Mathf.Min(zA.position.z, zB.position.z)));

        Max.x = Mathf.Max(xA.position.x, Mathf.Max(xB.position.x, Mathf.Max(zA.position.x, zB.position.x)));
        Max.y = Mathf.Max(xA.position.y, Mathf.Max(xB.position.y, Mathf.Max(zA.position.y, zB.position.y))) - EndYOffset;
        Max.z = -Mathf.Max(xA.position.z, Mathf.Max(xB.position.z, Mathf.Max(zA.position.z, zB.position.z)));
    }

    private void GetGameObjects()
    {
        if (xA == null)
            xA = transform.Find("xA");
        if (xB == null)
            xB = transform.Find("xB");
        if (zA == null)
            zA = transform.Find("zA");
        if (zB == null)
            zB = transform.Find("zB");
    }

    public void OnDrawGizmos()
    {
        GetGameObjects();
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
