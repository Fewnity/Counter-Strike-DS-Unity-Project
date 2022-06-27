using UnityEngine;

public class Collision : MonoBehaviour
{
    public BoxCollider m_BoxCollider;
    public bool ignoreRaycast = false;
    [Header("Zone index of the collision")]
    public int ZoneCollision;

    /// <summary>
    /// Get the position of the wall
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        Vector3 Pos = transform.position;
        Pos.x += m_BoxCollider.center.x;
        Pos.y += m_BoxCollider.center.y - 1;
        Pos.z += m_BoxCollider.center.z;
        Pos.z = -Pos.z;
        return Pos;
    }
}
