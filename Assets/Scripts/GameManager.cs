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
    }
    #endregion

    #region Game_variables
    private bool gamePaused = true;
    public int wandererCount = 10;
    public MoveEnum uniqueWandererMode;
    public GameObject wanderer;
    public GameObject uniqueWanderer;
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
        if (scene.name == "GameScene") SetupGame();
    }
    #endregion

    #region Gameplay_management
    #region Window_variables
    [HideInInspector]
    public float Xrange, Yrange; //Half the main camera dimenstions
    #endregion
    private void SetupGame()
    {
        Yrange = Camera.main.orthographicSize;
        Xrange = Yrange * Camera.main.aspect;

        //Spawn Wanderers
        for (int i = 0; i < wandererCount; i++)
        {
            Instantiate(wanderer, new Vector3(Random.Range(-Xrange, Xrange), Random.Range(-Yrange, Yrange), 0f), Quaternion.identity);
        }

        //Find spawn position for Unique wanderer where his path is contained in the screen
        GameObject _unqWanderer = Instantiate(uniqueWanderer);
        Vector2 buffer = _unqWanderer.transform.GetComponent<UniqueWanderer>().SetMode(uniqueWandererMode);
        Debug.Log(buffer);
        Vector3 SpawnPos = new Vector3(Random.Range(-Xrange, Xrange - buffer.x-2), Random.Range(-Yrange+buffer.y+2, Yrange), 0f);
        //Spawn Unique Wanderer
        _unqWanderer.transform.position = SpawnPos;

        //Setup Score
        scoreObject = GameObject.Find("/Canvas/ScoreDisplay/ScoreText");
        scoreText = scoreObject.GetComponent<TextMeshProUGUI>();

        gamePaused= false;
    }
    #endregion

    #region Score_variables
    [SerializeField] private int score = 1000;
    public int secondsBetweenTic = 1;
    public int ticIncrement = 5;
    public int punishIncrement = 50;

    private float scoreTimer = 0f;

    public GameObject scoreObject;
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