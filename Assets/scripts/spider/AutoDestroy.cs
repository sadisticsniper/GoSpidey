using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Tooltip("Time iin sec before this obj is destroyed.")]
    public float lifetime=1.5f;
    void Start()
    {
        Destroy(gameObject,lifetime);
    }

    
}
