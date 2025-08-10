using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rank : MonoBehaviour
{
    
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            transform.GetChild(i).GetChild(1).GetComponent<Text>().text = Main.Instance.rankData[i].Name + " - " + Main.Instance.rankData[i].Score.ToString("#,###");
        }
    }
    public void gomain()
    {
        SceneManager.LoadScene(0);
    }
}
