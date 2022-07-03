using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    public GameObject WayPointGM;
    public List<int> Edge = new List<int>();
}

public class PathFinding : MonoBehaviour
{
    //A matrix is : int[ROW][COL]
    //All the points on the map
    [HideInInspector]
    public List<Point> AllPoints = new List<Point>();
    private List<Waypoint> AllWaypoints = new List<Waypoint>();
    //Path found between StartWaypoint and FinalWaypoint
    private List<Waypoint> Path = new List<Waypoint>();
    //Matrix used to check if all points are reachable for all other points
    int[][] matrixCheckPow;
    //All matrices for 
    private List<PathLength> AllMatricesLength = new List<PathLength>();

    [Header("For testing")]
    //Index of the start waypoint for the path
    public int StartWaypoint;
    //Index of the final waypoint for the path
    public int FinalWaypoint;


    [Header("To copy in the code")]
    [TextArea(0, 6)]
    public string WaypointsCode;
    [TextArea(0, 6)]
    public string AllMatricesCode;

    [Header("Gizmo")]
    public Color PathsColor;
    public Color FoundPathColor;

    //Contains a matrix for a x path length
    [System.Serializable]
    public class PathLength
    {
        public int[][] matrixOneLength;
    }

    private void Awake()
    {
        AllPoints = new List<Point>(FindObjectsOfType<Point>().OrderBy(o => int.Parse(o.name.Substring(1))).ToList());
    }

    // Start is called before the first frame update
    void Start()
    {
        //Create matrix empty
        int[][] matrixA = MatrixCreate(AllPoints.Count, AllPoints.Count);
        matrixCheckPow = MatrixCreate(AllPoints.Count, AllPoints.Count);
        StringBuilder strBuilderWaipoints = new StringBuilder();

        //Create a waypoint for each point, (waypoint contains connected points to the current point)
        for (int i = 0; i < AllPoints.Count; i++)
        {
            Waypoint newWaypoint = new Waypoint();
            newWaypoint.WayPointGM = AllPoints[i].gameObject;
            strBuilderWaipoints.Append($"int edge{i}[{AllPoints[i].Edge.Count}] = ");
            strBuilderWaipoints.Append("{");

            //Add connected points to the waypoint
            for (int i2 = 0; i2 < AllPoints[i].Edge.Count; i2++)
            {
                int newEdge = AllPoints.IndexOf(AllPoints[i].Edge[i2]);
                newWaypoint.Edge.Add(AllPoints.IndexOf(AllPoints[i].Edge[i2]));
                //Add the connexion in the matrix
                matrixA[i][newEdge] = 1;

                if (i2 == AllPoints[i].Edge.Count - 1)
                    strBuilderWaipoints.Append($"{AllPoints.IndexOf(AllPoints[i].Edge[i2])}");
                else
                    strBuilderWaipoints.Append($"{AllPoints.IndexOf(AllPoints[i].Edge[i2])}:");
            }

            //Add the new waypoint in the list
            AllWaypoints.Add(newWaypoint);

            strBuilderWaipoints.Append("};\n");
            strBuilderWaipoints.Append($"CreateWaypoint({i}: {AllPoints[i].transform.position.x}: {AllPoints[i].transform.position.y}: {-AllPoints[i].transform.position.z}: {AllPoints[i].Edge.Count}: edge{i});\n");
        }
        WaypointsCode = strBuilderWaipoints.ToString();
        WaypointsCode = WaypointsCode.Replace(",", ".");
        WaypointsCode = WaypointsCode.Replace(":", ",");

        StringBuilder strBuilderWaipointsPow = new StringBuilder();
        //Check the max path length needed to navigate on the whole map
        int maxPow = 2;
        for (int pow = 1; pow < maxPow; pow++)
        {
            strBuilderWaipointsPow.Append($"int matrix{pow}[{AllPoints.Count}][{AllPoints.Count}] = ");
            strBuilderWaipointsPow.Append("{\n");

            //int[][] matrixACopy = MatrixDuplicate(matrixA);
            int[][] matrixACopy = MatrixPow(matrixA, pow);

            int aRows = matrixACopy.Length;
            int aCols = matrixACopy[0].Length;
            for (int row = 0; row < aRows; row++)
            {
                strBuilderWaipointsPow.Append("{");
                for (int col = 0; col < aCols; col++)
                {
                    if (matrixACopy[row][col] != 0)
                        matrixACopy[row][col] = 1;
                    strBuilderWaipointsPow.Append($"{matrixACopy[row][col]}");
                    if (col != aCols - 1)
                    {
                        strBuilderWaipointsPow.Append(",");
                    }

                    //Check if this point is reachable by all other points
                    // if matrixCheckPow[row][col] == 0, the point wasn't reachable before by all other points
                    // if matrixACopy[row][col] != 0, the point is reachable by all other points
                    if (matrixCheckPow[row][col] == 0 && matrixACopy[row][col] != 0)
                    {
                        //Mark this point as reachable by all other points
                        matrixCheckPow[row][col] = 1;
                    }
                }

                strBuilderWaipointsPow.Append("}");
                if (row != aRows - 1)
                {
                    strBuilderWaipointsPow.Append(",\n");
                }
            }

            AllMatricesCode = strBuilderWaipointsPow.ToString();
            AllMatricesCode += "};\n";
            AllMatricesCode += $"copyArrayToAllMatricesLength({aRows}, matrix{pow}, {pow - 1});\n\n";
            //Debug.Log($"Pow {pow}\n" + output);

            //Check if we need to create another matrix if some points are not reachable from a or some points
            for (int row = 0; row < aRows; row++)
            {
                for (int col = 0; col < aCols; col++)
                {
                    if (matrixCheckPow[row][col] == 0)
                    {
                        maxPow++;
                        //Break both for loop
                        row = aRows;
                        break;
                    }
                }
            }

            //Store this new matrix in the list
            PathLength newPathLength = new PathLength();
            newPathLength.matrixOneLength = matrixACopy;
            AllMatricesLength.Add(newPathLength);
        }

        //Start find a path between StartWaypoint and FinalWaypoint
        find();
    }

    void find()
    {
        //Check the minimal path length
        int pathLength = 0;
        for (int i2 = 0; i2 < AllMatricesLength.Count; i2++)
        {
            if (AllMatricesLength[i2].matrixOneLength[StartWaypoint][FinalWaypoint] != 0)
            {
                pathLength = i2;
                break;
            }
        }

        //Init pathfinding values
        int currentWaypoint = StartWaypoint;
        Path.Clear();

        //For two cases, do not make a scan
        //If the start waypoint and the final waypoint are the same, path is the start waypoint.
        if (StartWaypoint == FinalWaypoint)
        {
            Path.Add(AllWaypoints[currentWaypoint]);
        }
        else if (pathLength == 0)//If the length is 1 (pathLength = path length - 1), path contains the start waypoint and the final waypoint.
        {
            Path.Add(AllWaypoints[currentWaypoint]);
            Path.Add(AllWaypoints[FinalWaypoint]);
        }
        else
        {
            //If the path is not finished yet
            while (Path.Count != pathLength)
            {
                //Add verified waypoint to the path
                Path.Add(AllWaypoints[currentWaypoint]);

                do
                {
                    //Take a random connected point from the last tested waypoint
                    int NextWaypoint = Random.Range(0, AllWaypoints[currentWaypoint].Edge.Count);

                    //if the random connected point can reach the final point with (pathLength - Path.Count) movement
                    if (AllMatricesLength[pathLength - Path.Count].matrixOneLength[AllWaypoints[currentWaypoint].Edge[NextWaypoint]][FinalWaypoint] != 0)
                    {
                        currentWaypoint = AllWaypoints[currentWaypoint].Edge[NextWaypoint];
                        break;
                    }

                } while (true);
            }

            //Finalise the path
            Path.Add(AllWaypoints[currentWaypoint]);
            Path.Add(AllWaypoints[FinalWaypoint]);
        }
    }

    //Functions for matrices

    static int[][] MatrixCreate(int rows, int cols)
    {
        // creates a matrix initialized to all 0
        int[][] result = new int[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new int[cols]; // auto init to 0
        return result;
    }

    static int[][] MatrixPow(int[][] matrixA, int pow)
    {
        int[][] result = MatrixDuplicate(matrixA);

        for (int i = 1; i < pow; i++)
        {
            result = MatrixProduct(result, matrixA);
        }
        return result;
    }

    static int[][] MatrixDuplicate(int[][] matrix)
    {
        // assumes matrix is not null.
        int[][] result = MatrixCreate(matrix.Length, matrix[0].Length);
        for (int i = 0; i < matrix.Length; ++i) // copy the values
            for (int j = 0; j < matrix[i].Length; ++j)
                result[i][j] = matrix[i][j];
        return result;
    }

    static int[][] MatrixProduct(int[][] matrixA, int[][] matrixB)
    {
        int aRows = matrixA.Length; int aCols = matrixA[0].Length;
        int bRows = matrixB.Length; int bCols = matrixB[0].Length;

        int[][] result = MatrixCreate(aRows, bCols);
        Parallel.For(0, aRows, i =>
        {
            for (int j = 0; j < bCols; ++j)
                for (int k = 0; k < aCols; ++k)
                    result[i][j] += matrixA[i][k] * matrixB[k][j];
        }
        );
        return result;
    }
    /// <summary>
    /// Draw all paths
    /// </summary>
    private void OnDrawGizmos()
    {
        //Draw paths
        Gizmos.color = PathsColor;
        for (int i = 0; i < AllPoints.Count; i++)
        {
            for (int i2 = 0; i2 < AllPoints[i].Edge.Count; i2++)
            {
                Gizmos.DrawLine(AllPoints[i].transform.position, AllPoints[i].Edge[i2].transform.position);
            }
        }

        //Draw found path
        Gizmos.color = FoundPathColor;
        for (int i = 0; i < Path.Count - 1; i++)
        {
            Gizmos.DrawLine(Path[i].WayPointGM.transform.position, Path[i + 1].WayPointGM.transform.position);
        }
    }
}
