using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject Loadingpanel;
    public AudioSource clickSound;
    public AudioSource bgSound;
    public void PlayGame()
    {
        clickSound.Play();
        StartCoroutine(LoadLevelSequence());
        SceneManager.LoadScene("GameScene");
        bgSound.Play();
        IEnumerator LoadLevelSequence()
        {
            if (Loadingpanel != null){
                Loadingpanel.SetActive(true);
                bgSound.Stop();
            }
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

        
    }

    
    public void ExitGame()
    {
        Debug.Log("quit game");
        bgSound.Stop();
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
        #endif
    }
}
