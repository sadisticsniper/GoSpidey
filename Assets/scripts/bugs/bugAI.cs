using UnityEngine;

public class BugAI : MonoBehaviour  
{  
    [Header("Movement")]
    [SerializeField] private float speed = 2f;  
    [SerializeField] private float changeDirectionInterval = 2f; 
    [SerializeField] private float rotationSpeed = 10f; 
    [SerializeField] private  Animator animator ;

    [Header("Web Interaction")]
    [SerializeField] private float struggleIntensity = 0.05f; 
    [SerializeField] private float checkRadius = 0.2f; // Size of safety backup check
    [SerializeField] private LayerMask webLayer; // Optional: Set to 'Default' or 'Web' in inspector
    [SerializeField] private float escapeTime=6f;
    [SerializeField] private float trapChance =0.65f;
    

    private Vector2 direction;  
    private Rigidbody2D rb;  
    private float directionTimer;
    private bool isTrapped = false;
    private Vector3 webPosition;
    public bool IsTrapped => isTrapped;
    private float trappedTimer =0f;
    private float trapCooldown =0f;
      
    private void Awake()  
    {  
        rb = GetComponent<Rigidbody2D>();  
    }  

    private void Start()
    {
        ChooseRandomDirection();
    }
  
    private void Update()
    {
        if (isTrapped)
        {
            trappedTimer += Time.deltaTime;
            trapCooldown -= Time.deltaTime;
            Debug.Log(trappedTimer);
            StruggleInWeb();
            if(trappedTimer >= escapeTime){
                EscapeWeb();
            }
            return;
        }

        // Random roaming path timer
        directionTimer -= Time.deltaTime;
        if (directionTimer <= 0)
        {
            ChooseRandomDirection();
        }

        RotateTowardsDirection();
        BackupWebCheck();
    }

    private void FixedUpdate()  
    {  
        if (isTrapped) return;

        rb.linearVelocity = direction * speed;  
          
        // Screen boundary safety checks
        Vector3 pos = transform.position;  
        if (pos.x > 10f && direction.x > 0) direction.x *= -1;  
        else if (pos.x < -10f && direction.x < 0) direction.x *= -1;

        if (pos.y > 6f && direction.y > 0) direction.y *= -1;  
        else if (pos.y < -6f && direction.y < 0) direction.y *= -1;
    }  
  
    // Trigger Method (Works if Web has 'Is Trigger' checked)
    private void OnTriggerEnter2D(Collider2D collision)  
    {  
        if (isTrapped || trapCooldown>0) return;
        // if (collision.CompareTag("Web") || collision.gameObject.name.Contains("Web"))  
        if (collision.CompareTag("Web"))
        {  
            if(Random.value <= trapChance)
            {
                GetTrapped(collision.transform.position);
            }
        }    
    }  

    // Collision Method (Works if Web does NOT have 'Is Trigger' checked)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Web") || collision.gameObject.name.Contains("Web"))
        {
            GetTrapped(collision.transform.position);
        }
    }

    // Backup Safety Check (Looks for any overlapping web colliders directly beneath the bug)
    private void BackupWebCheck()
    {
        if (isTrapped) return;

        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        foreach (var col in overlappedColliders)
        {
            if (col.gameObject != this.gameObject && (col.CompareTag("Web") || col.gameObject.name.Contains("Web")))
            {
                GetTrapped(transform.position);
                break;
            }
        }
    }

    private void GetTrapped(Vector3 trapPoint)
    {
        if (isTrapped|| trapCooldown>0) return;

        isTrapped = true;
        rb.linearVelocity = Vector2.zero;  
        rb.bodyType = RigidbodyType2D.Kinematic; // Prevent further physics movement
        webPosition = transform.position; // Lock spot right here
        trappedTimer=0f;
        
        // Notify GameManager and Spider
        // GameManager gm = FindAnyObjectByType<GameManager>();
        // if (gm != null) gm.BugCaught();
    }

    private void EscapeWeb(){
        isTrapped=false;
        rb.bodyType = RigidbodyType2D.Dynamic;

        // push bug away from web
        Vector2 escapeDirection= Random.insideUnitCircle.normalized;

        rb.linearVelocity=escapeDirection*speed*2f;
        direction=escapeDirection;
        ChooseRandomDirection();
        transform.position=webPosition;
        trappedTimer =0f;
        trapCooldown=2f;
        Debug.Log("bug escaped!!");
    }

    private void ChooseRandomDirection()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        directionTimer = changeDirectionInterval + Random.Range(-0.5f, 0.5f);
    }

    private void RotateTowardsDirection()
    {
        if (direction == Vector2.zero) return;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void StruggleInWeb()
    {
        float offsetX = Random.Range(-struggleIntensity, struggleIntensity);
        float offsetY = Random.Range(-struggleIntensity, struggleIntensity);
        transform.position = webPosition + new Vector3(offsetX, offsetY, 0f);
        transform.Rotate(0, 0, Random.Range(-5f, 5f));
    }

    // Visualizes the backup safety circle in the Scene View editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}