using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct RankData
{
    public string Name;
    public float Score;
}
public class Main : MonoBehaviour
{
    public static Main Instance;

    public List<RankData> rankData;
    void Start()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;

        Load();
    }
    public void InitData()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }
    public void Load()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!PlayerPrefs.HasKey($"Score{i}"))
            {
                PlayerPrefs.SetFloat($"Score{i}", 0);
                PlayerPrefs.SetString($"Name{i}", "");
            }
            RankData data = new RankData();
            data.Score = PlayerPrefs.GetFloat($"Score{i}");
            data.Name = PlayerPrefs.GetString($"Name{i}");

            rankData.Add(data);
        }
        rankData = rankData.OrderByDescending(rankData => rankData.Score).ToList();
    }
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }
    public void gorank()
    {
        SceneManager.LoadScene(2);
    }
    public Transform Heler;
    public void Help()
    {
        Heler.gameObject.SetActive(!Heler.gameObject.activeSelf);
    }
    public void GameEnd()
    {
        Application.Quit();
    }
    public void Saver()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetFloat($"Score{i}", rankData[i].Score);
            PlayerPrefs.SetString($"Name{i}", rankData[i].Name);
        }
    }
    private void Update()
    {
    }
}
