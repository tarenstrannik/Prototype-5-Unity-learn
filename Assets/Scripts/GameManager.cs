using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textLivesCount;
    public TextMeshProUGUI textGameOver;
    public GameObject titleScreen;
    public GameObject pauseScreen;
    public GameObject slidePanel;
    public Button restartButton;

    public GameObject invisPlayerCursor;

    [SerializeField] private int score;
    public float spawnRate = 1f;
    private float curSpawnTimer=0;
    public int startLives = 3;
    private int lives;
    public bool isGameActive;
    public bool isGameOver = false;

    private List<GameObject> activeTargets = new List<GameObject> {  };



    // Start is called before the first frame update
    void Start()
    {
        lives = startLives;

    }

    // Update is called once per frame
    void Update()
    {
        if(!titleScreen.activeSelf && !isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                isGameActive = !isGameActive;
                pauseScreen.SetActive(!pauseScreen.activeSelf);
                slidePanel.SetActive(!slidePanel.activeSelf);
                if (Time.timeScale == 1)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
                //FreezeAllObjects();
            }
        }
    }
    void FreezeAllObjects()
    {
       foreach(GameObject target in activeTargets)
        {
            target.SendMessage("Pause", !isGameActive);
        }
    }

    IEnumerator SpawnTargets()
    {
        while(!isGameOver)
        {
            if (isGameActive)
            {
                if (curSpawnTimer <= 0)
                {
                    curSpawnTimer = spawnRate;
                    int indexTarget = UnityEngine.Random.Range(0, targets.Count);
                    GameObject targetTemp = Instantiate(targets[indexTarget]);
                    activeTargets.Add(targetTemp);
                }
                else
                {
                    curSpawnTimer -= Time.deltaTime;
                }
            }
            yield return null; 

        }
    }
    void RemoveFromList(GameObject target)
    {
        activeTargets.Remove(target);
    }
   public  void UpdateScore(int addScore)
    {
        score += addScore;
        textScore.text = "Score: " + score;
    }
    public void UpdateLives(int deltaLives)
    {
        lives -= deltaLives;
        textLivesCount.text = "Lives: " + lives;
    }
    public void LostOne()
    {
        if (isGameActive)
        {
            UpdateLives(1);
            if (lives <= 0) GameOver();
        }
    }
    private void GameOver()
    {
        isGameActive = false;
        isGameOver = true;
        textGameOver.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        slidePanel.gameObject.SetActive(false);
        Cursor.visible = true;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StartGame(int difficulty)
    {
        isGameActive = true;
        score = 0;
        spawnRate /= difficulty;
        lives = (int)Math.Ceiling((float)lives / difficulty);
        StartCoroutine(SpawnTargets());

        UpdateScore(0);
        UpdateLives(0);

        titleScreen.gameObject.SetActive(false);
        slidePanel.gameObject.SetActive(true);
    }

    public void ActivePlayerCursor(bool active)
    {
        invisPlayerCursor.gameObject.SetActive(active);
    }

    public void MoveInvisPlayerCursor(Vector3 pos)
    {
        invisPlayerCursor.transform.position = pos;
    }
    public void RotateInvisPlayerCursor(float angle)
    {
        invisPlayerCursor.transform.eulerAngles = new Vector3(0, 0, angle);
    }

}
