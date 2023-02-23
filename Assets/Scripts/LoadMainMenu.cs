using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainMenu : MonoBehaviour
{
    public void MainMenu()
    {
        //GameManager.Instance.ResetGame();
        //SceneManager.LoadScene("MainMenu");
        GameManager.Instance.MainMenu();
        
    }

}
