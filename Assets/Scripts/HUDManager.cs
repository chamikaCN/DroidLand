using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("many cameracontollers");
            return;
        }

        instance = this;
    }
    #endregion

    public GameObject gamePanel, pausePanel, completePanel;
    public Button blastButton, guardButton, pauseButton, resumeButton, menuButton, quitButton;
    public Slider slider;
    public Joystick moveJoystick, cameraJoystick;
    public Sprite victorySprite, defeatSprite;
    public HealthBar healthBar;
    public StatusBar statusBar;
    public TextMeshProUGUI playerCount, enemyCount;
    int totalDroids, playerDroids, enemyDroids;
    Color32 playerCol, enemyCol;

    void Start()
    {
        pausePanel.SetActive(false);
        completePanel.SetActive(false);
        gamePanel.SetActive(true);
        slider.value = 0.4f;
        playerCol = GameSceneManager.instance.getPlayerColour();
        enemyCol = GameSceneManager.instance.getEnemyColour();
        // statusBar.setStatusBarColurs(playerCol, enemyCol);
        //playerCount.faceColor = playerCol;
        // enemyCount.faceColor = enemyCol;


    }

    void Update()
    {
        CameraJoystickUpdate();
        MoveJoystickUpdate();
    }

    public void SliderValueUpdate()
    {
        CameraController.instance.CalculateCameraZoom(slider.value);
    }

    void CameraJoystickUpdate()
    {
        float hor = cameraJoystick.Horizontal, ver = cameraJoystick.Vertical;
        if (Mathf.Abs(ver * hor) > 0)
        {
            CameraController.instance.CalculateCameraMovement(hor, ver);
        }
    }

    void MoveJoystickUpdate()
    {
        Vector3 move = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
        PlayerManager.instance.PlayerMovement(move);
    }

    public void AttackButtonUpdate()
    {
        PlayerManager.instance.PlayerAttack();
    }

    public void ChangeButtonUpdate()
    {
        PlayerManager.instance.PlayerChange();
    }

    public void BlastButtonUpdate()
    {
        bool blasted = PlayerManager.instance.PlayerBlast();
        if (blasted)
        {
            blastButton.gameObject.SetActive(false);
            StartCoroutine(blastButtonReset());
        }
    }

    public void GuardButtonUpdate()
    {
        bool guardOn = PlayerManager.instance.PlayerGuard();
        if (guardOn)
        {
            guardButton.gameObject.SetActive(false);
            StartCoroutine(guardButtonReset());
        }
    }

    IEnumerator blastButtonReset()
    {
        yield return new WaitForSeconds(8);
        blastButton.gameObject.SetActive(true);
    }

    IEnumerator guardButtonReset()
    {
        yield return new WaitForSeconds(15);
        guardButton.gameObject.SetActive(true);
    }

    public void Pause()
    {
        gamePanel.SetActive(false);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        gamePanel.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        GameSceneManager.instance.LoadMenu();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void Victory()
    {
        displayCompletion(victorySprite, "Victory !!!");
    }

    public void Defeat()
    {
        displayCompletion(defeatSprite, "Defeat !!!");
    }

    public void displayCompletion(Sprite im, string msg)
    {
        completePanel.GetComponent<Image>().sprite = im;
        completePanel.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        gamePanel.SetActive(false);
        completePanel.SetActive(true);
    }

    public void EndGame()
    {
        GameSceneManager.instance.LoadMenu();
    }

    public void setMaxHealth(int max)
    {
        healthBar.setMaxHealth(max);
    }

    public void setHealth(int health)
    {
        healthBar.changeHealth(health);
    }

    public void setPlayerDroids(int newplayerDroids)
    {
        playerDroids = newplayerDroids;
        playerCount.text = playerDroids.ToString();
        statusBar.setTotalDroids(playerDroids + enemyDroids);
        statusBar.changePlayerDroids(playerDroids);

    }

    public void setEnemyDroids(int newEnemyDroids)
    {
        enemyDroids = newEnemyDroids;
        enemyCount.text = enemyDroids.ToString();
        statusBar.setTotalDroids(playerDroids + enemyDroids);
    }

}
