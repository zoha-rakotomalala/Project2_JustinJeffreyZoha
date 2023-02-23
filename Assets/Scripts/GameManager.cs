using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public GameObject poofPrefab;
    GameObject leaveButton;
    GameObject endMessage;

    #region Unity_functions
    private void Awake()
    {
        if (Instance== null)
        {
            Instance= this;
        }else if(Instance!=this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!gamePaused)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer > secondsBetweenTic)
            {
                scoreTimer -= secondsBetweenTic;
                TicScore();
            }
        }
        if (score <= 0)
        {
            TriggerLose();
        }
    }
    #endregion

    #region Game_variables
    private bool gamePaused = true;
    public int wandererCount = 10;
    public MoveEnum uniqueWandererMode;
    public GameObject wandererPrefab;
    public GameObject uniqueWandererPrefab;
    private GameObject[] Wanderers;
    #endregion

    #region Scene_transitions
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Called after the scene has finished loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " Loaded");
        if (scene.name == "GameScene") { 
            SetupGame();
            leaveButton = GameObject.FindWithTag("LeaveButton");
            leaveButton.SetActive(false);
            endMessage = GameObject.FindWithTag("EndMessage");
            endMessage.SetActive(false);
            Debug.Log("Leave button: " + leaveButton);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*public void ResetGame()
    {
        // Reset game variables
        gamePaused = true;
        wandererCount = 10;
        uniqueWandererMode = MoveEnum.Square;
        score = 1000;
        secondsBetweenTic = 1;
        ticIncrement = 5;
        punishIncrement = 50;

        // Destroy existing wanderers and unique wanderer
        foreach (GameObject wan in Wanderers)
        {
            Destroy(wan);
        }
        GameObject[] uniqueWanderers = GameObject.FindGameObjectsWithTag("UniqueWanderer");
        foreach (GameObject uniqueWanderer in uniqueWanderers)
        {
            Destroy(uniqueWanderer);
        }
    }*/
    #endregion

    #region Gameplay_management
    #region Window_variables
    [HideInInspector]
    public float Xrange, Ymax, Ymin; //Half the main camera dimenstions
    #endregion
    private void SetupGame()
    {
        Ymax = Camera.main.orthographicSize;
        Ymin = -Ymax * 5 / 8;
        Xrange = Ymax * Camera.main.aspect;

        Wanderers = new GameObject[wandererCount];
        //Spawn Wanderers
        for (int i = 0; i < wandererCount; i++)
        {
            Wanderers[i] = Instantiate(wandererPrefab, new Vector3(Random.Range(-Xrange, Xrange), Random.Range(Ymin, Ymax), 0f), Quaternion.identity);
        }

        //Find spawn position for Unique wanderer where his path is contained in the screen
        GameObject _unqWanderer = Instantiate(uniqueWandererPrefab);
        Vector2 buffer = _unqWanderer.transform.GetComponent<UniqueWanderer>().SetMode(uniqueWandererMode);


        Vector3 SpawnPos = new Vector3(Random.Range(-Xrange, Xrange - buffer.x), Random.Range(Ymin+buffer.y, Ymax), 0f);
        //Spawn Unique Wanderer
        _unqWanderer.transform.position = SpawnPos;

        //Setup Score
        scoreObject = GameObject.Find("/Canvas/ScoreDisplay/ScoreText");
        scoreText = scoreObject.GetComponent<TextMeshProUGUI>();

        gamePaused= false;
    }

    public void TriggerWin()
    {
        PoofWanderers();
        gamePaused = true;
        leaveButton.SetActive(true);
        endMessage.GetComponent<TextMeshProUGUI>().text = "You won!";
        endMessage.SetActive(true) ;
    }

    private void TriggerLose() {
        leaveButton.SetActive(true);
        endMessage.GetComponent<TextMeshProUGUI>().text = "You lose!";
        endMessage.SetActive(true);
    }
    private void PoofWanderers()
    {
        foreach(GameObject wan in Wanderers){
            Instantiate(poofPrefab, wan.transform.position, Quaternion.identity);
            Destroy(wan);
        }
    }
    #endregion

    #region Score_variables
    [SerializeField] private int score = 1000;
    public int secondsBetweenTic = 1;
    public int ticIncrement = 5;
    public int punishIncrement = 50;

    private float scoreTimer = 0f;

    private GameObject scoreObject;
    private TextMeshProUGUI scoreText;
    #endregion

    #region Score_functions
    private void TicScore()
    {
        score -= ticIncrement;
        if(score <= 0)
        {
            score = 0;
        }
        UpdateScoreUI();
    }
    public void DecreaseScore()
    {
        score -= punishIncrement;
        if (score <= 0)
        {
            score = 0;
        }
        UpdateScoreUI();
    }
    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }
    #endregion
}