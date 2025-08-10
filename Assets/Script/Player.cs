using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
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
                GameManager.Instance.GameEnd(true);
            }
            HpBar.value = hp / maxhp;
            HpText.text = "플레이어 체력 " + hp.ToString("#,##0") + " / " + maxhp.ToString("#,##0");
        }
    }

    public float hp;
    public float maxhp;

    public bool Invin;

    public float Speed;
    public float Damage;

    public float AttackSpeed;

    public TowerSetting SetTower;
    public Turret InfoTower;

    public Slider HpBar;
    public Text HpText;

    public float attacktime=1;

    public static float Level=0;

    public int level=0;

    public Animator attackmotion;
    void Start()
    {
        HpText = HpBar.transform.parent.GetComponentInChildren<Text>();
    }
    public RaycastHit hit;
    public void Init()
    {
        level = 0;
        attacktime = 1;
        Hp = maxhp;
    }
    public bool repair;

    public Transform Shield;
    void Update()
    {
        if (repair)
        {
            if (!GameObject.FindAnyObjectByType<Turret>())
            {
                repair = false;
            }
            else RRRepair.gameObject.SetActive(true);
        }
        else
        {
            RRRepair.gameObject.SetActive(false);
        }
        if (Level >= 1)
        {
            Level--;
            level++;
            Damage += 2;
            AttackSpeed += 0.25f;
            maxhp += 3;
            hp += 3;
            GameManager.Instance.LogPrint("<color=green>플레이어 레벨업! 공격력+2, 공격속도+25%, 체력+3</color>");
            GameManager.Instance.LogPrint("<color=green>스코어 + 100 (사유 : 플레이어 레벨업)</color>");
            GameManager.Instance.Score += 100f;
        }
        if (InfoTower)
        {
            TowerInfo.gameObject.SetActive(true);
            switch (InfoTower.Type)
            {
                case TowerType.Nomal:
                    TowerInfo.GetChild(0).GetComponent<Text>().text = "일반 타워";
                    break;
                case TowerType.Lazer:
                    TowerInfo.GetChild(0).GetComponent<Text>().text = "레이저 타워";
                    break;
                case TowerType.Heal:
                    TowerInfo.GetChild(0).GetComponent<Text>().text = "수리 타워";
                    break;
                case TowerType.Big:
                    TowerInfo.GetChild(0).GetComponent<Text>().text = "대포 타워";
                    break;
            }
            TowerInfo.GetChild(1).GetChild(0).GetComponent<Slider>().value = InfoTower.Hp / InfoTower.maxhp;
            TowerInfo.GetChild(1).GetChild(0).GetComponentInChildren<Text>().text = InfoTower.Hp.ToString("#,##0") +" / "  + InfoTower.maxhp.ToString("#,##0");
            TowerInfo.GetChild(4).GetComponent<Text>().text = "강화단계 : " + InfoTower.Level + "단계";
            TowerInfo.GetChild(5).GetComponent<Text>().text = "힘 : " + InfoTower.Power;
            TowerInfo.GetChild(6).GetComponent<Text>().text = "발사 딜레이 : " + InfoTower.AttackDelay;
            TowerInfo.GetChild(7).GetComponent<Text>().text = "발사 속도 : " + InfoTower.AttackRate;
            TowerInfo.GetChild(8).GetComponent<Text>().text = "부품" + ((InfoTower.Level+1)*2).ToString("#,##0")+"개";
            TowerInfo.GetChild(9).GetComponent<Text>().text = "부품" + ((InfoTower.Level + 1) * 5).ToString("#,##0") + "개";
        }
        else
        {
            InfoClose();
        }

        if (attacktime >= 1)
        {
            attackmotion.SetTrigger("Attack");
        }
        else attacktime += Time.deltaTime * AttackSpeed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (attacktime >= 1)
            {
                attacktime = 0;
                attackmotion.GetComponent<BulletMove>().Damage = Damage;
                attackmotion.SetFloat("AttackSpeed", AttackSpeed / 2f);
                attackmotion.SetTrigger("Attack");
            }
        }

        if (SetTower)
        {
            SetTower.transform.position = transform.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (SetTower.Ok)
                {
                    this.type = -1;
                    SetTower.Set();
                    SetTower = null;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(r, out hit, 10, LayerMask.GetMask("Tower")))
            {
                if (hit.transform.GetComponent<Turret>())
                {
                    if (!repair)
                    {
                        if (InfoTower != hit.transform.GetComponent<Turret>())
                        {
                            InfoTower = hit.transform.GetComponent<Turret>();
                            if (InfoTower.Type == TowerType.FakeWall)
                            {
                                InfoClose();
                            }
                        }
                        else
                        {
                            InfoClose();
                        }
                    }
                    else
                    {
                        hit.transform.GetComponent<Turret>().Requir();
                        repair = false;
                    }
                }
            }
            else
            {
                InfoTower = null;
            }
        }
        for(int i = 0; i < 5; i++)
        {
            if (Input.GetKeyDown((KeyCode)(257+i)))
            {
                SettingTower(i);
            }
        }
        for(int i = 0; i < 2; i++)
        {
            if (Input.GetKeyDown((KeyCode)(262+i)))
            {
                UseItem(i);
            }
        }
        if(GameManager.Instance.ItemCounts[2] > 0)
        {
            GameManager.Instance.ItemCounts[2]--;
            Hp += 20;
        }
        if (GameManager.Instance.ItemCounts[3] > 0)
        {
            GameManager.Instance.ItemCounts[3]--;
            repair = true;
        }
    }
    public Transform RRRepair;
    public void UseItem(int Type)
    {
        if (GameManager.Instance.ItemCounts[Type] > 0)
        {
            GameManager.Instance.ItemCounts[Type]--;
            switch (Type)
            {
                case 0:
                    StopCoroutine(speed());
                    StartCoroutine(speed());
                    break;
                case 1:
                    StopCoroutine(shield());
                    StartCoroutine(shield()); break;
            }
        }
        else
        {
            GameManager.Instance.LogPrint("<color=red>아이템이 부족합니다.</color>");
        }
    }
    IEnumerator speed()
    {
        GetComponent<PlayerMove>().item = true;
        yield return new WaitForSeconds(4);
        GetComponent<PlayerMove>().item = false;
    }
    IEnumerator shield()
    {
        Invin = true;
        Shield.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Shield.gameObject.SetActive(false);
        Invin = false;
    }
    public Transform TowerInfo;
    public void InfoClose()
    {
        TowerInfo.gameObject.SetActive(false);
        InfoTower = null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<EnemyBase>())
        {
            Hp -= collision.transform.GetComponent<EnemyBase>().Damage;
        }
    }
    public int type;

    public GameObject[] Towers;
    public void SettingTower(int type)
    {
        if (SetTower)
        {
            GameManager.Instance.TowerCounts[this.type]++;
            Destroy(SetTower.gameObject);
        }
        if (GameManager.Instance.TowerCounts[type] > 0&& type != this.type)
        {
            GameManager.Instance.TowerCounts[type]--;
            SetTower = Instantiate(Towers[type]).GetComponent<TowerSetting>();
            this.type = type;
        }
        else
        {
            if(GameManager.Instance.TowerCounts[type] <= 0)
            {
                GameManager.Instance.LogPrint("<color=red>해당 타워를 가지고 있지 않습니다.</color>");
            }
            this.type = -1;
            SetTower = null;
        }
    }

    public void TowerUp()
    {
        InfoTower.Upgrade();
    }
    public void TowerRe()
    {
        InfoTower.Requir();
    }
}
