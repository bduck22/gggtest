using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiPrint : MonoBehaviour
{
    public bool level;
    public bool parts;
    public Text Text;
    public string defaultstring;
    void Start()
    {
        Text = GetComponent<Text>();
    }

    void Update()
    {
        if (level)
        {
            Text.text = defaultstring + GameManager.Instance.player.level;
        }
        else if (parts) {
            Text.text = defaultstring + GameManager.Instance.Parts + "°³";
        }
        else
        {
            Text.text = defaultstring + GameManager.Instance.Score.ToString("#,##0") + "Á¡";
        }
    }
}
