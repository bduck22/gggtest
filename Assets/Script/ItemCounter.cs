using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCounter : MonoBehaviour
{
    public ItemType Type;
    public Text Text;
    public string defaulttext;
    void Start()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        Text.text = defaulttext + GameManager.Instance.ItemCounts[(int)Type] + "°³";
    }
}
