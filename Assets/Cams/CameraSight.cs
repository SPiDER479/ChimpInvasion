using System;
using System.Collections.Generic;
using UnityEngine;
public class CameraSight : MonoBehaviour
{
    private PlayerMovement player;
    private LayerMask playerMask, wallMask, enemyMask;
    [SerializeField] private float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;
    [SerializeField] private float meshResolution, enemyAlertRadius;
    [SerializeField] private MeshFilter viewMeshFilter;
    private AudioSource beep;
    [SerializeField] private Material berserkMat, stealthMat;
    private Mesh viewMesh;
    private void OnEnable()
    {
        playerMask = LayerMask.GetMask("Player");
        enemyMask = LayerMask.GetMask("Enemy");
        wallMask = LayerMask.GetMask("Wall");
        beep = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (player.gameMode == "Stealth")
            viewMeshFilter.GetComponent<MeshRenderer>().material = stealthMat;
        else if (player.gameMode == "Berserk")
            viewMeshFilter.GetComponent<MeshRenderer>().material = berserkMat;
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
                    if (player.gameMode == "Stealth")
                        player.levelEnd(false);
                    else if (player.gameMode == "Berserk")
                    {
                        Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, enemyAlertRadius, enemyMask);
                        foreach (Collider enemy in enemyInRadius)
                        {
                            enemy.GetComponent<EnemyPlayerSpotted>().enabled = false;
                            enemy.GetComponent<EnemyPlayerSpotted>().enabled = true;
                        }
                        if (!beep.isPlaying)
                            beep.Play();
                    }
                }
            }
        }
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
