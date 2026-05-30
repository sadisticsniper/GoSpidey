
using UnityEngine;

public class SpiderMovement : MonoBehaviour
{
    [SerializeField] private  Animator animator ;
    [SerializeField] private float eatRange =1f;
    [SerializeField] private KeyCode eatKey =  KeyCode.E;
    [SerializeField] private float energyGain =10f;
    [SerializeField] private  GameObject eatVFXprefab;

    public float moveSpeed =8f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;
    public AudioSource moveSound;

    public bool Ismoving(){
        return movement.magnitude>0.1f;
        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager=FindAnyObjectByType<GameManager>();
        spriteRenderer =GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x= Input.GetAxisRaw("Horizontal");
        movement.y= Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(eatKey)){
            TryEatBug();
        }

        if( movement.x != 0||movement.y !=0 ){
            animator.SetBool("isRunning", true);
            if (!moveSound.isPlaying)
            {
                moveSound.Play();
            }
        }
        else{
            animator.SetBool("isRunning", false);
            moveSound.Stop();
        }
        if (movement.x <0){
            spriteRenderer.flipX=false;
        }
        else if(movement.x>0){
            spriteRenderer.flipX=true;
        }
        movement.Normalize();
    }

    void FixedUpdate()
    {
        rb.linearVelocity =movement*moveSpeed;
    }

    private void TryEatBug()
    {
        Collider2D[]hits =Physics2D.OverlapCircleAll(transform.position,eatRange);
        foreach(Collider2D hit in hits){
            BugAI bug= hit.GetComponent<BugAI>();

            if (bug != null && bug.IsTrapped){
                EatBug(bug);
                return;
            }
        }
    }

    private void EatBug(BugAI bug){
        if(gameManager != null){
            gameManager.GainEnergy(energyGain);
            gameManager.BugCaught();
            Instantiate(eatVFXprefab,transform.position,Quaternion.identity);
        }
        Destroy(bug.gameObject);
    }

   
}