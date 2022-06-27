using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Exporter : MonoBehaviour
{
    private List<Stairs> AllStairs = new List<Stairs>();
    private List<Collision> AllCollisions = new List<Collision>();
    private List<Trigger> AllTriggers = new List<Trigger>();
    private List<ShadowTrigger> AllShadowTriggers = new List<ShadowTrigger>();
    [Header("To fill")]
    public PathFinding pathFinding;
    public BombTrigger AZone;
    public BombTrigger BZone;
    [Header("To copy in the code")]
    [TextArea(0, 6)]
    public string StairsCode;
    [TextArea(0, 6)]
    public string CollisionsCode;
    [TextArea(0, 6)]
    public string TriggersCode;
    [TextArea(0, 6)]
    public string ShadowsTriggersCode;
    [TextArea(0, 6)]
    public string ZonesCode;

    // Start is called before the first frame update
    void Start()
    {
        AllStairs = new List<Stairs>(FindObjectsOfType<Stairs>().OrderBy(o => int.Parse(o.name.Substring(6))).ToList());
        AllCollisions = new List<Collision>(FindObjectsOfType<Collision>().OrderBy(o => int.Parse(o.name.Substring(4))).ToList());
        AllShadowTriggers = new List<ShadowTrigger>(FindObjectsOfType<ShadowTrigger>().OrderBy(o => int.Parse(o.name.Substring(13))).ToList());
        AllTriggers = new List<Trigger>(FindObjectsOfType<Trigger>().OrderBy(o => int.Parse(o.name.Split(" ")[0].Substring(7))).ToList());

        StringBuilder strBuilderStairs = new StringBuilder();

        if (AllStairs.Count != 0)
        {
            for (int i = 0; i < AllStairs.Count; i++)
            {
                Vector3 Min, Max;
                AllStairs[i].GetMinMax(out Min, out Max);
                strBuilderStairs.Append($"CreateStairs({Min.x}: {Max.x}: {Max.z}: {Min.z}: {Min.y}: {Max.y}: {AllStairs[i].Direction}: {i});\n");
            }
            StairsCode = strBuilderStairs.ToString();
            StairsCode = StairsCode.Remove(StairsCode.Length - 1);
            StairsCode = StairsCode.Replace(",", ".");
            StairsCode = StairsCode.Replace(":", ",");
            StairsCode = $"allMaps[mapToSet].StairsCount = {AllStairs.Count};\n" + StairsCode;
            //Debug.Log(StairsFinal);
        }

        if (AllCollisions.Count != 0)
        {
            StringBuilder strBuilderCollisions = new StringBuilder();
            for (int i = 0; i < AllCollisions.Count; i++)
            {
                Vector3 Pos = AllCollisions[i].GetPosition();
                strBuilderCollisions.Append($"CreateWall({Pos.x}: {Pos.y}: {Pos.z}: {AllCollisions[i].m_BoxCollider.size.x}: {AllCollisions[i].m_BoxCollider.size.y}: {AllCollisions[i].m_BoxCollider.size.z}: {AllCollisions[i].ZoneCollision}: {i});\n");
            }
            CollisionsCode = strBuilderCollisions.ToString();
            CollisionsCode = CollisionsCode.Remove(CollisionsCode.Length - 1);
            CollisionsCode = CollisionsCode.Replace(",", ".");
            CollisionsCode = CollisionsCode.Replace(":", ",");
            //Debug.Log(CollisionsFinal);
        }

        if (AllTriggers.Count != 0)
        {
            StringBuilder strBuilderTriggers = new StringBuilder();
            for (int i = 0; i < AllTriggers.Count; i++)
            {
                Vector3 Pos = AllTriggers[i].GetPosition();
                strBuilderTriggers.Append($"CalculateTriggerColBox({Pos.x}: {Pos.z}: {AllTriggers[i].m_BoxCollider.size.x}: {AllTriggers[i].m_BoxCollider.size.z}: {i});\n");
            }
            TriggersCode = strBuilderTriggers.ToString();
            TriggersCode = TriggersCode.Remove(TriggersCode.Length - 1);
            TriggersCode = TriggersCode.Replace(",", ".");
            TriggersCode = TriggersCode.Replace(":", ",");
            //Debug.Log(TrigerFinal);
        }

        if(AllShadowTriggers.Count != 0)
        {
            StringBuilder strBuilderShadowTriggers = new StringBuilder();
            for (int i = 0; i < AllShadowTriggers.Count; i++)
            {
                Vector3 Pos = AllShadowTriggers[i].GetPosition();
                strBuilderShadowTriggers.Append($"CalculateShadowColBox({Pos.x}: {Pos.y}: {Pos.z}: {AllShadowTriggers[i].m_BoxCollider.size.x}: {AllShadowTriggers[i].m_BoxCollider.size.y}: {AllShadowTriggers[i].m_BoxCollider.size.z}: {i});\n");
            }
            ShadowsTriggersCode = strBuilderShadowTriggers.ToString();
            ShadowsTriggersCode = ShadowsTriggersCode.Remove(ShadowsTriggersCode.Length - 1);
            ShadowsTriggersCode = ShadowsTriggersCode.Replace(",", ".");
            ShadowsTriggersCode = ShadowsTriggersCode.Replace(":", ",");
            //Debug.Log(ShadowTrigerFinal);
        }

        if (AZone != null)
        {
            Vector3 APos = AZone.GetPosition();
            ZonesCode += $"SetBombZone({APos.x}: {APos.z}: {AZone.m_BoxCollider.size.x}: {AZone.m_BoxCollider.size.z}: {0}: {pathFinding.AllPoints.IndexOf(AZone.connectedPoint)});\n";
        }
        if (BZone != null)
        {
            Vector3 BPos = BZone.GetPosition();
            ZonesCode += $"SetBombZone({BPos.x}: {BPos.z}: {BZone.m_BoxCollider.size.x}: {BZone.m_BoxCollider.size.z}: {1}: {pathFinding.AllPoints.IndexOf(BZone.connectedPoint)});\n";
        }
        if (!string.IsNullOrEmpty(ZonesCode))
        {
            ZonesCode = ZonesCode.Remove(ZonesCode.Length - 1);
            ZonesCode = ZonesCode.Replace(",", ".");
            ZonesCode = ZonesCode.Replace(":", ",");
            //Debug.Log(ZoneFinal);
        }
    }
}
