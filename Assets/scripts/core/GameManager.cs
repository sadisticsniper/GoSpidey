using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Bug Spawning")]
    [SerializeField] private GameObject bugPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3f;

    [Header("Game Stats")]
    [SerializeField] private float energy = 100f;
    [SerializeField] private int bugsTrapped = 0;
    [SerializeField] private int bugsToWin = 10;

    [Header("Energy System")]
    [SerializeField] private float passiveDrain=2f;
    [SerializeField] private float movementDrain=1.5f;

    [Header("UI")]
    [SerializeField] private Slider energyBar;
    [SerializeField] private TMP_Text bugText;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameVictoryPanel;

    private float spawnTimer;
    
    private bool isGameOver = false;
    public AudioSource bgMusic;
    public AudioSource gameOverSound ;
    public AudioSource gameWinSound ;
    public AudioSource bugEatSound ;

    private void Update()
    {
        if (isGameOver)
            return;

        HandleSpawning();
        HandleEnergyDrain();
        CheckWinLose();
        UpdateUI();
        DrainEnergy();
    }

    private void Start()
    {
        UpdateUI();
        resultText.gameObject.SetActive(false);
        bugEatSound.Stop();
    }
    private void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnBug();
            spawnTimer = 0f;
        }
    }

    private void SpawnBug()
    {
        if (spawnPoints.Length == 0)
            return;
        int index =Random.Range(0,spawnPoints.Length);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(bugPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void HandleEnergyDrain()
    {
        // energy -= Time.deltaTime * 2f;
        // Rigidbody2D spiderRb= FindAnyObjectByType<SpiderMovement>().GetComponent<Rigidbody2D>();

        energy -= passiveDrain*Time.deltaTime;
        SpiderMovement spider=FindAnyObjectByType<SpiderMovement>();
        if(spider!=null&&spider.Ismoving()){
            energy-= movementDrain*Time.deltaTime;
        }
        energy =Mathf.Clamp(energy,0f,100f);

        // if (spiderRb.linearVelocity.magnitude > 0.1f){
        //     energy -= Time.deltaTime * 5f;
    }
    

    private void DrainEnergy()
    {
        energy -= passiveDrain*Time.deltaTime;
        SpiderMovement spider=FindAnyObjectByType<SpiderMovement>();
        if(spider!=null&&spider.Ismoving()){
            energy-= movementDrain*Time.deltaTime;
        }
        energy =Mathf.Clamp(energy,0f,100f);
    }

    public void BugCaught()
    {
        bugsTrapped++;
        energy += 15f;

        if (energy > 100f)
            energy = 100f;

        Debug.Log("Bug caught! total: "+bugsTrapped);
        bugEatSound.Play();
    }

    private void UpdateUI()
    {
        energyBar.value=energy;
        bugText.text= $"Bugs: {bugsTrapped}/{bugsToWin}";
    }

    private void CheckWinLose()
    {
        if (bugsTrapped >= bugsToWin)
        {
            isGameOver = true;
            // resultText.gameObject.SetActive(true);
            // resultText.text="YOU WON ";
            gameVictoryPanel.SetActive(true);
            gameWinSound.Play();
            bgMusic.Stop();
            Time.timeScale=0f;
        }

        if (energy <= 0)
        {
            isGameOver = true;
            // resultText.gameObject.SetActive(true);
            // resultText.text="YOU LOSE ";
            Time.timeScale=0f;
            gameOverPanel.SetActive(true);
            bgMusic.Stop();
            gameOverSound.Play();
        }
    
    }
    public void GainEnergy(float amount){
    energy+=amount;

    if(energy > 100f)
        energy=100f;
    }
    public void RestartGame(){
        Time.timeScale=1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GoToMainMenu(){
        Time.timeScale=1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame(){
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
        #endif
    }
}