using System.Collections;
using System.Collections.Generic;
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
    #endregion

    #region Game_variables
    public int wandererCount = 10;
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

        //Spawn Unique Wanderer
        Instantiate(uniqueWanderer, new Vector3(Random.Range(-Xrange, Xrange), Random.Range(-Yrange, Yrange), 0f), Quaternion.identity);
    }
    #endregion
}
