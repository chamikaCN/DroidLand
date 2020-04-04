using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamSelector : MonoBehaviour
{
    public void LoadGame(string team)
    {
        PlayerPrefs.SetString("Team", team);
        SceneManager.LoadScene("DevScene");
    }
}
