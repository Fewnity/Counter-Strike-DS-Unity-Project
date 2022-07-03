using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Point : MonoBehaviour
{
    public int zone;
    public List<Point> Edge = new List<Point>();
}
