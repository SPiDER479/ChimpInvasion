using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class EnemySight : MonoBehaviour
{
    private string gameMode;
    private LayerMask playerMask, wallMask;
    [SerializeField] private float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;
    [SerializeField] private float meshResolution;
    [SerializeField] private MeshFilter viewMeshFilter;
    private EnemyChase ec;
    private EnemyDefault ed;
    private EnemyPlayerSpotted eps;
    private Mesh viewMesh;
    [SerializeField] private Material berserkMat, stealthMat;
    private void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
        wallMask = LayerMask.GetMask("Wall");
        ec = GetComponent<EnemyChase>();
        ed = GetComponent<EnemyDefault>();
        eps = GetComponent<EnemyPlayerSpotted>();
        StreamReader r = new StreamReader("currentlevel.txt");
        int levelNumber = int.Parse(r.ReadLine());
        r.Close();
        if (levelNumber % 8 <= 4 && levelNumber % 8 >= 1)
        {
            gameMode = "Berserk";
            viewMeshFilter.GetComponent<MeshRenderer>().material = berserkMat;
        }
        else
        {
            gameMode = "Stealth";
            viewMeshFilter.GetComponent<MeshRenderer>().material = stealthMat;
        }
    }
    private void OnEnable()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }
    private void Update()
    {
        Collider[] playerInRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        if (playerInRadius.Length == 1)
        {
            Transform playerTransform = playerInRadius[0].transform;
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, wallMask))
                {
                    if (gameMode == "Stealth")
                        GameObject.Find("Player").GetComponent<PlayerMovement>().levelEnd(false);
                    else if (gameMode == "Berserk")
                    {
                        ed.enabled = false;
                        eps.enabled = false;
                        ec.enabled = true;
                    }
                }
                else
                    ec.enabled = false;
            }
        }
        else
            ec.enabled = false;
    }
    private void LateUpdate()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
        }
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = new Vector3(Mathf.Sin(globalAngle * Mathf.Deg2Rad), 0,
            Mathf.Cos(globalAngle * Mathf.Deg2Rad));
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, wallMask))
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
    }
    private struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst, angle;
        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
}
