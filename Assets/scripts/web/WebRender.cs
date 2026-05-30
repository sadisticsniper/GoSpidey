using UnityEngine;

public class WebRenderer : MonoBehaviour
{
    [SerializeField] private GameObject webNodePrefab;
    [SerializeField] private GameObject webConnectionPrefab;
    [SerializeField] private Transform WebParent;

    private WebNode selectedNode;
    private int nextNodeID = 0;

    private void Update()
    {
        HandleNodePlacement();
        HandleNodeConnection();
    }
    void Start(){
        GameObject sceneContainer=GameObject.Find("WebParent");
        if(sceneContainer != null)
        {
            WebParent=sceneContainer.transform;
        }
        else{
            Debug.LogError("couldnt find webparent");
        }
    }
    private void HandleNodePlacement()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 mousePos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;

        GameObject nodeObj =
            Instantiate(
                webNodePrefab,
                mousePos,
                Quaternion.identity,
                WebParent
            );

            

        WebNode node =
            nodeObj.GetComponent<WebNode>();

        node.Initialize(nextNodeID);

        nextNodeID++;
    }

    private void HandleNodeConnection()
    {
        if (!Input.GetMouseButtonDown(1))
            return;

        Vector3 mousePos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;

        Collider2D hit =
            Physics2D.OverlapPoint(mousePos);

        if (hit == null)
            return;

        WebNode clickedNode =
            hit.GetComponent<WebNode>();

        if (clickedNode == null)
            return;

        if (selectedNode == null)
        {
            SelectNode(clickedNode);
        }
        else
        {
            if (selectedNode != clickedNode)
            {
                CreateConnection(
                    selectedNode,
                    clickedNode
                );
            }

            DeselectNode();
        }
    }

    private void SelectNode(WebNode node)
    {
        selectedNode = node;

        SpriteRenderer sr =
            node.GetComponent<SpriteRenderer>();

        sr.color = Color.yellow;
    }

    private void DeselectNode()
    {
        if (selectedNode == null)
            return;

        SpriteRenderer sr =
            selectedNode.GetComponent<SpriteRenderer>();

        sr.color = Color.white;

        selectedNode = null;
    }

    private void CreateConnection(
        WebNode nodeA,
        WebNode nodeB)
    {
        GameObject connectionObj =
            Instantiate(
                webConnectionPrefab,
                Vector3.zero,
                Quaternion.identity,
                WebParent
            );

        WebConnection connection =
            connectionObj.GetComponent<WebConnection>();

        connection.Initialize(
            nodeA,
            nodeB
        );
    }
}