using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }
    }

    public GameObject gameoverCanvas;

    public TextMeshProUGUI titleText;
    public int enemyNumber;
    private void Awake()
    {
        instance = this;
    }

    public bool isPlaying;
    void Start()
    {
        isPlaying = true;
    }

    public void GameEnd()
    {
        isPlaying = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameoverCanvas.SetActive(true);
    }

    public void EnemyDie()
    {
        enemyNumber--;
        if (enemyNumber == 0)
        {
            titleText.text = "You Win";
            GameEnd();
        }
    }

    public void PlayeDie()
    {
        titleText.text = "You Died";
        GameEnd();
    }

    public void AgainPressed()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitPressed()
    {
        Application.Quit();
    }
}
