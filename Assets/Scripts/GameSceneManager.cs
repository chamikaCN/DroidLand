using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    #region Singleton
    public static GameSceneManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("many gamemanagers");
            return;
        }

        instance = this;
    }
    #endregion

    List<string> teams;
    String playerTeam;
    Droid[] droids;
    List<Droid> playerDroids, enemyDroids;
    Droid currentDroid;
    public Color32 orange, blue;
    Color32 playerColour, enemyColour;
    void Start()
    {
        teams = new List<string>() { "Orange", "Blue" };
        playerTeam = PlayerPrefs.GetString("Team");
        teams.Remove(playerTeam);
        playerDroids = new List<Droid>();
        enemyDroids = new List<Droid>();

        CameraController.instance.changeOffset(playerTeam);
        setColours(playerTeam);

        droids = GameObject.FindObjectsOfType<Droid>();
        foreach (Droid droid in droids)
        {
            if (droid.tag.ToString() == playerTeam)
            {
                playerDroids.Add(droid);
            }
            else
            {
                enemyDroids.Add(droid);
            }
        }
        int rand = UnityEngine.Random.Range(0, playerDroids.Count);
        currentDroid = playerDroids[rand];
        PlayerManager.instance.setCurrentDroid(currentDroid);
        currentDroid.setPlayerControl();
        CameraController.instance.setTransform(currentDroid.transform);
        currentDroid.IncreaseHealthBar();
        HUDManager.instance.setPlayerDroids(playerDroids.Count);
        HUDManager.instance.setEnemyDroids(enemyDroids.Count);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeDroid();
        }

    }

    void setColours(string team)
    {
        if (playerTeam == "Orange")
        {
            playerColour = orange;
            enemyColour = blue;
        }
        else if (playerTeam == "Blue")
        {
            playerColour = blue;
            enemyColour = orange;
        }
    }

    public void ChangeDroid()
    {
        //becuase of using random droid doesnt change on all clicks
        currentDroid.DecreaseHealthBar();
        currentDroid.ResetPlayerControl();
        PlayerManager.instance.removeCurrentDroid();
        int rand = UnityEngine.Random.Range(0, playerDroids.Count);
        currentDroid = playerDroids[rand];
        PlayerManager.instance.setCurrentDroid(currentDroid);
        currentDroid.setPlayerControl();
        currentDroid.IncreaseHealthBar();
        CameraController.instance.setTransform(currentDroid.transform);
    }

    public Droid getCurrentDroid()
    {
        return currentDroid;
    }

    public string getTeam()
    {
        return playerTeam;
    }

    public Color32 getPlayerColour(){
        return playerColour;
    }

    public Color32 getEnemyColour()
    {
        return enemyColour;
    }

    public void DestroyCheck(Droid droid)
    {
        if (droid == GameSceneManager.instance.getCurrentDroid())
        {
            foreach (Droid newDroid in enemyDroids)
            {
                newDroid.RemoveEnemy(droid);
            }
            if (playerDroids.Count > 1)
            {
                playerDroids.Remove(currentDroid);
                GameSceneManager.instance.ChangeDroid();
            }
            else
            {
                StartCoroutine(RealizationWait("D"));
            }
            HUDManager.instance.setPlayerDroids(playerDroids.Count);
        }
        else if (droid.getTeam() == playerTeam)
        {
            playerDroids.Remove(droid);
            foreach (Droid newDroid1 in enemyDroids)
            {
                newDroid1.RemoveEnemy(droid);
            }
            HUDManager.instance.setPlayerDroids(playerDroids.Count);
        }
        else
        {
            if (enemyDroids.Count > 1)
            {
                enemyDroids.Remove(droid);
                foreach (Droid newDroid2 in playerDroids)
                {
                    newDroid2.RemoveEnemy(droid);
                }
            }
            else { StartCoroutine(RealizationWait("V")); }
            HUDManager.instance.setEnemyDroids(enemyDroids.Count);
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("WelcomeScene");
    }

    IEnumerator RealizationWait(string sign)
    {
        yield return new WaitForSeconds(2f);
        if (sign == "V")
        {
            HUDManager.instance.Victory();
        }
        else if (sign == "D")
        {
            HUDManager.instance.Defeat();
        }
    }

}
