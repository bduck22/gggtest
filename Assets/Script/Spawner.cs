using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public EnemyBase[] Prefabs;

    public int Wave=0;

    public void WaveStart()
    {
        Wave++;
        Boss = false;
        Waving = true;
        wavetime = 0;
        spawncount =0;
        if (Wave == 3)
        {
            SpawnTiming *= 2;
        }
    }

    public float wavetime;

    public float SpawnTiming;

    public float spawncount;

    public bool Waving = false;

    public bool Boss;
    void Start()
    {
    }
    void Update()
    {
        if (Waving)
        {
            wavetime += Time.deltaTime;
            switch (Wave)
            {
                case 1: 
                    if(wavetime > 120&&!Boss)
                    {
                        Boss = true;
                        Spawn(4);
                    }
                    break;
                case 2:
                    if (wavetime > 120 && !Boss)
                    {
                        Boss = true;
                        Spawn(5);
                    }
                    break;
                case 3:
                    if (!Boss)
                    {
                        Boss = true;
                        Spawn(6);
                    }
                    if (wavetime > 180)
                    {
                        GameManager.Instance.GameEnd(true);
                    }
                    break;
            }
            if(wavetime >= (SpawnTiming * spawncount-spawncount*0.1f))
            {
                if(wavetime >= 120)
                {
                    Spawn(Random.Range(0, 4));
                }
                else Spawn(Random.Range(0, ((int)(wavetime / 30)+1)));
                spawncount++;
            }
            GameManager.Instance.timer.text = (Mathf.FloorToInt(wavetime / 60f)).ToString("#,##0") + "Ка " + (wavetime % 60).ToString("#,##0") + "УЪ";
        }
    }
    public void Spawn(int Type)
    {
        int wid = Random.Range(0, 4);
        GameObject M = Instantiate(Prefabs[Type].gameObject, transform.GetChild(wid).position, Quaternion.identity);
        M.GetComponent<EnemyBase>().maxhp += spawncount * 0.02f;
        M.GetComponent<EnemyBase>().Damage += spawncount * 0.05f;
        M.GetComponent<EnemyBase>().Init();
    }
    public void AllKill()
    {
        foreach(EnemyBase g in GameObject.FindObjectsOfType<EnemyBase>())
        {
            Destroy(g.HpBar.transform.parent.gameObject);
            Destroy(g.gameObject);
        }
    }
}
