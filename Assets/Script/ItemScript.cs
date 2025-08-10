using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Speed,
    Sheild,
    Heal,
    Repair
}
public class ItemScript : MonoBehaviour
{
    public ItemType Type;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            GameManager.Instance.LogPrint($"<color=green>���� + 200 (���� : ������ ȹ��)</color>");
            GameManager.Instance.ItemCounts[(int)Type]++;
            Destroy(gameObject);
        }
    }
}
