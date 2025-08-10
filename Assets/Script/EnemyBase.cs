using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyType
{
    Shot,
    Range,
    Big,
    Dash,
    B1,
    B2,
    B3
}
public class EnemyBase : MonoBehaviour
{
    public Transform Target;

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
            if (value <= 0)
            {
                switch (Type)
                {
                    case EnemyType.Shot:
                        GameManager.Instance.Parts += 1f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 10 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 10f;
                        Player.Level += 0.05f;
                        break;
                    case EnemyType.Range:
                        GameManager.Instance.Parts += 3f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 12 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 12f;
                        Player.Level += 0.05f;
                        break;
                    case EnemyType.Big:
                        GameManager.Instance.Parts += 7f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 15 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 15f;
                        Player.Level += 0.1f;
                        break;
                    case EnemyType.Dash:
                        GameManager.Instance.Parts += 6f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 20 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 20f;
                        Player.Level += 0.05f;
                        break;
                    case EnemyType.B1:
                        GameManager.Instance.Parts += 50f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 200 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 200f;
                        Player.Level += 0.5f;
                        GameManager.Instance.rest();
                        break;
                    case EnemyType.B2:
                        GameManager.Instance.Parts += 100f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 300 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.Score += 300f;
                        Player.Level ++;
                        GameManager.Instance.rest();
                        break;
                    case EnemyType.B3:
                        GameManager.Instance.Score += 500f;
                        GameManager.Instance.LogPrint("<color=green>스코어 + 500 (사유 : 몬스터 처치)</color>");
                        GameManager.Instance.GameEnd(false);
                        break;
                }
                float R = Random.Range(0f, 10f);
                if(R < 3f)
                {
                    GameManager.Instance.SpawnItem(transform.position);
                }
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

    public float Speed;
    public float Damage;

    public float AttackSpeed;
    public float AttackDelay;

    public float LoadTime;

    public float Intersection;

    public EnemyType Type;

    public float attacktime = 0;
    public float delaytime = 0;

    public bool Hit = false;

    public float BossTime;
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        LoadTime = Random.Range(0.1f, 0.3f);
        InvokeRepeating("LoadTarget", 0.25f, LoadTime);
    }
    Rigidbody Rigidbody;
    void Update()
    {
        BossTime += Time.deltaTime;
        HpBar.transform.parent.position = HpP.position;
        if (Target)
        {
            transform.LookAt(Target);
            if (Type != EnemyType.Dash)
            {
                Rigidbody.velocity = Vector3.zero;
                if ((Intersection == 0 && !Hit) || (Intersection > 0 && Vector3.Distance(transform.position, Target.position) > Intersection))
                {
                    attacktime = 0;
                    delaytime = 0;
                    transform.position = Vector3.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);
                }
                else
                {
                    if (delaytime > AttackDelay)
                    {
                        if (attacktime >= 1)
                        {
                            attacktime = 0;
                            Attack();
                        }
                        else attacktime += Time.deltaTime * AttackSpeed;
                    }
                    else delaytime += Time.deltaTime;
                }
            }
            else
            {
                if (delaytime > AttackDelay)
                {
                    if (attacktime >= 1)
                    {
                        Rigidbody.velocity = Vector3.zero;
                        attacktime = 0;
                        Rigidbody.AddForce((Target.position - transform.position).normalized * Speed, ForceMode.Impulse);
                    }
                    else attacktime += Time.deltaTime * AttackSpeed;
                }
                else delaytime += Time.deltaTime;
            }
        }
    }

    public void Init()
    {
        attacktime = 0;
        delaytime = 0;
        HpBar = GameManager.Instance.GetBar();
        HpText = HpBar.transform.parent.GetComponentInChildren<Text>();
        Hp = maxhp;
    }

    public BulletMove Bullet;
    public BulletMove Bullet2;

    public Transform[] ShotP;
    public Transform SubShot;
    public void Attack()
    {
        StartCoroutine(AttackMotion());
        if (Type != EnemyType.B1 && Type != EnemyType.B2 && Type != EnemyType.B3)
        {
            if (Target.GetComponent<Turret>())
            {
                Target.GetComponent<Turret>().Hp -= Damage;
            }
            else if (Target.GetComponent<Player>())
            {
                Target.GetComponent<Player>().Hp -= Damage;
            }
            else if (Target.GetComponent<GameManager>())
            {
                GameManager.Instance.Hp -= Damage;
            }
        }
    }
    IEnumerator AttackMotion()
    {
        foreach (Transform P in ShotP)
        {
            GameObject B;
            switch (Type)
            {
                case EnemyType.Range:
                    B = Instantiate(Bullet.gameObject, P.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().Target = Target.transform;
                    break;
                case EnemyType.B1:
                    B = Instantiate(Bullet.gameObject, P.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().Damage = Damage + BossTime * 0.2f;
                    B.GetComponent<BulletMove>().TargetP = Target.transform.position;
                    break;
                case EnemyType.B2:
                    B = Instantiate(Bullet.gameObject, P.position, transform.rotation);
                    B.GetComponent<EffectDestroyer>().Damage = Damage;
                    break;
                case EnemyType.B3:
                    B = Instantiate(Bullet.gameObject, P.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().Damage = Damage;
                    B.GetComponent<BulletMove>().TargetP = Target.transform.position;
                    B = Instantiate(Bullet2.gameObject, SubShot.position, Quaternion.identity);
                    B.GetComponent<BulletMove>().Target = Target.transform;
                    B.GetComponent<BulletMove>().Damage = Damage;
                    yield return new WaitForSeconds(0.1f);
                    break;
            }
        }
        yield return null;
    }

    public void LoadTarget()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Object"))
        {
            if (Target)
            {
                if (Vector3.Distance(Target.position, transform.position) > Vector3.Distance(g.transform.parent.position, transform.position))
                {
                    Target = g.transform.parent;
                }
            }
            else
            {
                Target = g.transform.parent;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (Type == EnemyType.Dash && collision.gameObject.layer != 6)
        {
            Rigidbody.AddForce((collision.transform.position - transform.position).normalized * -Speed / 1.5f, ForceMode.Impulse);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterHit"))
        {
            Hp -= other.transform.root.GetComponentInChildren<BulletMove>().Damage;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Hit = false;
        if (collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Object"))
            {
                Hit = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.childCount > 0)
        {
            if (collision.transform.GetChild(0).CompareTag("Object"))
            {
                Hit = false;
            }
        }
    }
}
