using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public int rank=-1;

    public Transform NameInput;
    private void Start()
    {
        Time.timeScale = 0;
        for(int i= Main.Instance.rankData.Count-1; i>=0; i--)
        {
            if (Main.Instance.rankData[i].Score <= GameManager.Instance.Score)
            {
                rank = i;   
                NameInput.gameObject.SetActive(true);
            }
        }
    }
    public void Save()
    {
        if (rank != -1)
        {
            RankData data = new RankData();
            data.Score = GameManager.Instance.Score;
            data.Name = NameInput.GetComponent<InputField>().text;
            Main.Instance.rankData[rank] = data;
        }
        Main.Instance.Saver();
    }
    public void gomain()
    {
        Save();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void gorank()
    {
        Save();
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
}
