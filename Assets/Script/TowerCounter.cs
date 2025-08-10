using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerCounter : MonoBehaviour
{
    public TowerType Type;
    public Text Text;
    public string defaulttext;
    void Start()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        Text.text = defaulttext + GameManager.Instance.TowerCounts[(int)Type] + "°³";
    }
}
