using UnityEngine;

public class ShadowTrigger : MonoBehaviour
{
    public Vector3 FinalOffset;
    public BoxCollider m_BoxCollider;

    /// <summary>
    /// Get the position of the trigger
    /// </summary>
    /// <returns>Position</returns>
    public Vector3 GetPosition()
    {
        Vector3 Pos = transform.position;
        Pos.x += FinalOffset.x + m_BoxCollider.center.x;
        Pos.y += FinalOffset.y + m_BoxCollider.center.y;
        Pos.z += FinalOffset.z + m_BoxCollider.center.z;
        Pos.z = -Pos.z;
        return Pos;
    }
}
