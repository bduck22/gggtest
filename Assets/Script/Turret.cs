using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TowerType
{
    Nomal,
    Lazer,
    Heal,
    Big,
    FakeWall
}
public class Turret : MonoBehaviour
{
    public EnemyBase Target;

    public Transform Head;

    public Transform[] ShotP;

    public float Hp
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            if (value > maxhp)
            {
                hp = maxhp;
            }
            if(value <= 0)
            {
                Destroy(HpBar.transform.parent.gameObject);
                Destroy(gameObject);
                hp = 0;
            }
            HpBar.value = hp / maxhp;
            HpText.text = hp.ToString("#,##0") + " / " + maxhp.ToString("#,##0");
        }
    }

    public float hp;
    public float maxhp;

    public Transform HpP;
    public Slider HpBar;
    public Text HpText;

    public float AttackDelay;
    public float AttackRate;
    public float Power;

    public float delaytime=0;
    public float ratetime=1;

    public TowerType Type;

    public List<Turret> Turrets;

    public float lazertime;

    public bool Stop;

    public int Level=0;
    void Start()
    {
        HpBar = GameManager.Instance.GetBar();
        HpText = HpBar.transform.parent.GetComponentInChildren<Text>();
        Init();
    }
    public void Init()
    {
        delaytime = 0;
        ratetime = 1;
        lazertime = 0;
        Hp = maxhp;
    }
    void Update()
    {        
        HpBar.transform.parent.position = HpP.position;
        if (Type != TowerType.FakeWall&&!Stop)
        {
            if (Type != TowerType.Heal)
            {
                if (Target)
                {
                    Head.LookAt(Target.transform);
                    if (delaytime > AttackDelay)
                    {
                        lazertime += Time.deltaTime;
                        if (ratetime >= 1)
                        {
                            ratetime = 0;
                            Attack();
                        }
                        else ratetime += Time.deltaTime * AttackRate;
                    }
                    else delaytime += Time.deltaTime;
                }
                else
                {
                    if (Type == TowerType.Lazer)
                    {
                        Bullet.transform.gameObject.SetActive(false);
                    }
                    lazertime = 0;
                    delaytime = 0;
                    ratetime = 1;
                }
            }
            else
            {
                if (ratetime >= 1)
                {
                    ratetime = 0;
                    Attack();
                }
                else ratetime += Time.deltaTime * AttackRate;
            }
        }
        else
        {
            if (Type == TowerType.Lazer)
            {
                Bullet.transform.gameObject.SetActive(false);
            }
            lazertime = 0;
            delaytime = 0;
            ratetime = 1;
        }
    }
    public BulletMove Bullet;
    void Attack()
    {
        StartCoroutine(AttackMotion());
        if (Type != TowerType.Heal)
        {
            if (Type != TowerType.Big)
            {
                Target.Hp -= Power+(Type==TowerType.Lazer?lazertime*0.25f:0);
            }
        }
        else
        {
            foreach (Turret t in Turrets)
            {
                t.Hp += Power;
            }
        }
    }
    IEnumerator AttackMotion()
    {
        foreach(Transform P in ShotP)
        {
            GameObject B;
            switch (Type)
            {
                case TowerType.Nomal:
                    B = Instantiate(Bullet.gameObject, P.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().Target = Target.transform;
                    yield return new WaitForSeconds(0.05f);
                    break;
                case TowerType.Lazer:
                    Bullet.transform.gameObject.SetActive(true);
                    Bullet.transform.GetComponent<LineRenderer>().SetPosition(0, P.position);
                    Bullet.transform.GetComponent<LineRenderer>().SetPosition(1, Target.transform.position);
                    break;
                case TowerType.Big:
                    B = Instantiate(Bullet.gameObject, P.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().TargetP = Target.transform.position;
                    B.GetComponent<BulletMove>().Damage = Power;
                    break;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Enemy"))
            {
                Hp -= collision.transform.GetComponent<EnemyBase>().Damage;
            }
        }
    }

    public void Upgrade()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        if(GameManager.Instance.IsBuy((Level + 1) * 5))
        {
            GameManager.Instance.Score += 50;
            GameManager.Instance.LogPrint("<color=green>���ھ� + 50 (���� : Ÿ�� ��ȭ)</color>");
            Level++;
            switch (Type)
            {
                case TowerType.Nomal:
                    Power += 2;
                    AttackDelay -= 0.1f;
                    AttackRate += 0.2f;
                    maxhp += 2;
                    Hp += 2;
                    GameManager.Instance.LogPrint("<color=blue>�Ϲ� Ÿ�� ��ȭ ����! ��+2, �߻������-0.1, �߻�ӵ�+20%,ü��+2</color>");
                    break;
                case TowerType.Lazer:
                    Power += 0.1f;
                    AttackDelay -= 0.1f;
                    AttackRate += 1.5f;
                    maxhp += 3;
                    Hp += 3;
                    GameManager.Instance.LogPrint("<color=blue>������ Ÿ�� ��ȭ ����! ��+0.1f, �߻������-0.1, �߻�ӵ�+150%,ü��+3</color>");
                    break;
                case TowerType.Heal:
                    Power += 2;
                    AttackRate += 0.2f;
                    maxhp += 2;
                    Hp += 2;
                    GameManager.Instance.LogPrint("<color=blue>���� Ÿ�� ��ȭ ����! ��+2, �߻�ӵ�+20%,ü��+2</color>");
                    break;
                case TowerType.Big:
                    Power += 5;
                    AttackDelay -= 0.1f;
                    AttackRate += 0.1f;
                    maxhp += 3;
                    Hp += 3;
                    GameManager.Instance.LogPrint("<color=blue>���� Ÿ�� ��ȭ ����! ��+5, �߻������-0.1, �߻�ӵ�+10%,ü��+3</color>");
                    break;
            }
        }
    }

    public void Requir()
    {
        if (GameManager.Instance.IsBuy((Level + 1) * 2))
        {
            Hp += maxhp;
            GameManager.Instance.LogPrint("<color=blue>Ÿ�� ���� �Ϸ�</color>");
        }
    }

    public IEnumerator stoptower()
    {
        Debug.Log(transform.parent.name + "�۵�����");
        Stop = true;
        yield return new WaitForSeconds(2f);
        Debug.Log(transform.parent.name + "�۵��簳");
        Stop = false;
    }
}
