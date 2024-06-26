using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSight : MonoBehaviour
{
    private LayerMask enemyMask, wallMask;
    public float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;
    public float meshResolution;
    [SerializeField] private MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    private bool knifeThrown;
    private void OnEnable()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        wallMask = LayerMask.GetMask("Wall");
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        knifeThrown = false;
    }
    private void Update()
    {
        Collider[] targetInRadius = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
        foreach (Collider target in targetInRadius)
        {
            Transform targetTransform = target.transform;
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, targetTransform.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, wallMask) && !knifeThrown)
                {
                    GameObject e = KnifePool.SharedInstance.GetPooledObject();
                    if (e != null)
                    {
                        knifeThrown = true;
                        StartCoroutine(KnifeThrown());
                        e.transform.position = GetComponentInParent<Transform>().position;
                        e.transform.LookAt(targetTransform);
                        e.SetActive(true);
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
    IEnumerator KnifeThrown()
    {
        yield return new WaitForSeconds(0.5f);
        knifeThrown = false;
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
