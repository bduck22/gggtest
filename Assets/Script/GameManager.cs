using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float Score;

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            if (value >= hp || (value < hp && !Invin))
            {
                hp = value;
            }
            if (value > maxhp)
            {
                hp = maxhp;
            }
            if (value <= 0)
            {
                hp = 0;
                GameEnd(true);
            }
            HpBar.value = hp / maxhp;
            HpText.text = "�ھ� ü�� " + hp.ToString("#,##0") + " / " + maxhp.ToString("#,##0");
        }
    }

    public float hp;
    public float maxhp;

    public bool Invin;

    public GameObject HpBarPrefab;

    public Transform HpBarCanvas;

    public int[] TowerCounts;

    public int[] ItemCounts;

    public Slider HpBar;
    public Text HpText;

    public float Parts;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Slider GetBar()
    {
        return Instantiate(HpBarPrefab, HpBarCanvas).GetComponentInChildren<Slider>();
    }

    public Spawner spawner;
    void Start()
    {
        HpText = HpBar.transform.parent.GetComponentInChildren<Text>();
        Hp = maxhp;
        player.Init();
        spawner.WaveStart();
    }
    public Text timer;

    public Player player;
    public void rest()
    {
        StartCoroutine(Rest());
    }
    public IEnumerator Rest()
    {
        Score += 1000 * (Hp / maxhp);
        LogPrint("<color=blue>���̺� ����!</color>");
        LogPrint($"<color=green>���� �ھ� ü�� : {(Hp / maxhp)*100}% ���� + {1000 * (Hp / maxhp)}</color>");
        player.Hp += maxhp * 0.3f;
        player.Hp += maxhp * 0.1f;
        spawner.Waving = false;
        spawner.Boss = false;
        spawner.AllKill();
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(1f);
            timer.text = (10 - i).ToString() + "��";
        }
        spawner.WaveStart();
    }

    public GameObject[] ItemPrefabs;
    public void SpawnItem(Vector3 P)
    {
        Instantiate(ItemPrefabs[UnityEngine.Random.Range(0, ItemPrefabs.Length)].gameObject, P, Quaternion.identity);
    }

    public Transform result;
    public void GameEnd(bool over)
    {
        Time.timeScale = 0;
        
        if (over)
        {
            result.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            result.GetChild(1).gameObject.SetActive(true);
        }
        result.gameObject.SetActive(true);
    }
    public Transform Setting;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Setting.gameObject.SetActive(!Setting.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            LogPrint("<color=black>ġƮ : �÷��̾�� ��ȣ Ĩ ü�� 100% ȸ��</color>");
            player.Hp += maxhp;
            Hp += maxhp;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (Invin)
            {
                LogPrint("<color=black>ġƮ : �÷��̾�� ��ȣ Ĩ ��������</color>");
                player.Invin = false;
                Invin = false;
            }
            else
            {
                LogPrint("<color=black>ġƮ : �÷��̾�� ��ȣ Ĩ ����</color>");
                player.Invin = true;
                Invin = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            LogPrint("<color=black>ġƮ : ��ǰ 100�� �߰�</color>");
            Parts += 100;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            LogPrint("<color=black>ġƮ : ���� ������ ����</color>");
            SpawnItem(new Vector3(2, 0, 2));
        }
        if (Input.GetKeyDown(KeyCode.F5)){
            if (spawner.Wave < 3)
            {
                LogPrint("<color=black>ġƮ : ���� ���̺�� �̵�</color>");
                spawner.Waving = false;
                spawner.Boss = false;
                spawner.AllKill();
                spawner.WaveStart();
            }
            else
            {
                LogPrint("<color=black>ġƮ : ������ ���̺�</color>");
            }
        }
        if (Input.GetKey(KeyCode.F6))
        {
            Time.timeScale = 0;
        }
        else if (Input.GetKeyUp(KeyCode.F6))
        {
            Time.timeScale = 1;
        }
    }

    public void BuyTower(int Type)
    {
        TowerType T = (TowerType)Type;
        switch (T)
        {
            case TowerType.Nomal:
                if (IsBuy(7))
                {
                    LogPrint("<color=white>�Ϲ� Ÿ�� ���� ����</color>");
                    TowerCounts[Type]++;
                }
                break;
            case TowerType.Lazer:
                if (IsBuy(10))
                {
                    LogPrint("<color=white>������ Ÿ�� ���� ����</color>");
                    TowerCounts[Type]++;
                }
                break;
            case TowerType.Heal:
                if (IsBuy(15))
                {
                    LogPrint("<color=white>���� Ÿ�� ���� ����</color>");
                    TowerCounts[Type]++;
                }
                break;
            case TowerType.Big:
                if (IsBuy(10))
                {
                    LogPrint("<color=white>���� Ÿ�� ���� ����</color>");
                    TowerCounts[Type]++;
                }
                break;
            case TowerType.FakeWall:
                if (IsBuy(10))
                {
                    LogPrint("<color=white>���� ���� ����</color>");
                    TowerCounts[Type]++;
                }
                break;
        }
    }


    public Transform Logfeild;
    public Text LogPrefab;
    public void LogPrint(string log)
    {
        Instantiate(LogPrefab, Logfeild).text = log;
    }

    public Transform Store;
    public void OpenStore()
    {
        Store.gameObject.SetActive(!Store.gameObject.activeSelf);
    }

    public bool IsBuy(float price)
    {
        if (price > Parts)
        {
            LogPrint("<color=red>��ǰ�� �����մϴ�.</color>");
            return false;
        }
        else
        {
            Parts -= price;
            return true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Enemy"))
            {
                if (collision.transform.GetComponent<EnemyBase>().Type != EnemyType.Range)
                {
                    LogPrint("<color=red>�ھ� ���� ����</color>");
                    Hp -= collision.transform.GetComponent<EnemyBase>().Damage;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ObjectHit"))
        {
            LogPrint("<color=red>�ھ� ���� ����</color>");
            Hp -= other.transform.parent.GetComponent<BulletMove>().Damage;
        }
    }
    public void GameEnd()
    {
        Application.Quit();
    }
    public void gomain()
    {
        SceneManager.LoadScene(0);
    }
}
