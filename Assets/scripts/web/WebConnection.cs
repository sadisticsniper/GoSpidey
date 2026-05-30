using UnityEngine;

public class WebConnection : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private WebNode nodeA;
    private WebNode nodeB;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        edgeCollider=GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.02f;
    }
    public void Initialize(WebNode a,WebNode b)
    {
        nodeA = a;
        nodeB =b;

        UpdateLine();
        
    }
    private void Update()
    {
        UpdateLine();
        // UpdateCollider(Vector2 a,Vector2 b);
    }
    private void UpdateLine()
    {
        if(nodeA==null|| nodeB==null)
            return;
        lineRenderer.positionCount=2;

        Vector3 a=nodeA.transform.position;
        Vector3 b=nodeB.transform.position;
        lineRenderer.SetPosition(0,a);
        lineRenderer.SetPosition(1,b);

        UpdateCollider(a,b);
    }
    private void UpdateCollider(Vector2 a,Vector2 b)
    {
        if (edgeCollider==null) return;

        Vector2[] points = new Vector2[2];
        points[0]=a;
        points[1]=b;

        edgeCollider.SetPoints(new System.Collections.Generic.List<Vector2>(points));
    }
}
