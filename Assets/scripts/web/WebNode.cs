using UnityEngine;

public class WebNode : MonoBehaviour
{
    public int nodeID;
    public Vector2 position;

    private bool isActive = true;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        position = transform.position;
    }

    public void Initialize(int id)
    {
        nodeID = id;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void Damage()
    {
        isActive = false;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }
    }

    public void Repair()
    {
        isActive = true;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }
}