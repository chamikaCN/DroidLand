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
    void Start()
    {
        teams = new List<string>() { "Orange", "Blue" };
        playerTeam = PlayerPrefs.GetString("Team");
        teams.Remove(playerTeam);
        playerDroids = new List<Droid>();
        enemyDroids = new List<Droid>();

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeDroid();
        }

    }

    public void ChangeDroid()
    {
        //becuase of using random droid doesnt change on all clicks
        currentDroid.ResetPlayerControl();
        PlayerManager.instance.removeCurrentDroid();
        int rand = UnityEngine.Random.Range(0, playerDroids.Count);
        currentDroid = playerDroids[rand];
        PlayerManager.instance.setCurrentDroid(currentDroid);
        currentDroid.setPlayerControl();
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
                SceneManager.LoadScene("WelcomeScene");
            }
        }
        else if (droid.getTeam() == playerTeam)
        {
            playerDroids.Remove(droid);
            foreach (Droid newDroid1 in enemyDroids)
            {
                newDroid1.RemoveEnemy(droid);
            }
        }
        else
        {
            enemyDroids.Remove(droid);
            foreach (Droid newDroid2 in playerDroids)
            {
                newDroid2.RemoveEnemy(droid);
            }
        }
    }
}
