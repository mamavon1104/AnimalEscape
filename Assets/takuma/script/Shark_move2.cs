using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shark_move2 : MonoBehaviour
{
    public enum Shark_Condition
    {
        Patrolling,//巡廻
        Battle,//戦闘
    }
    public Shark_Condition Condition;

    private GameObject player;
    private GameObject shark;

    private int pat_num;//巡廻場所の数
    private int current_pos_num = 0;//現在移動している場所
    public float Detection_distance = 10;//探知範囲
    public float move_speed = 0.08f;
    public List<GameObject> Pat_pos_list = new List<GameObject>();//巡廻位置
    void Start()
    {
        Condition = Shark_Condition.Patrolling;
        //↓Find処理
        player = GameObject.Find("Player");
        shark = transform.Find("Shark").gameObject;

        //巡廻地点取得
        Transform Main = transform.parent;
        Transform Patrolling_position = Main.Find("Patrolling_position");
        pat_num = Patrolling_position.childCount;
        for (int i = 0; i < pat_num; i++)
        {
            Transform child = Patrolling_position.GetChild(i);
            Pat_pos_list.Add(child.gameObject);
        }
    }
    void Update()
    {
        //Playerとの距離を測って追う対称を変える
        if ((player.transform.position - shark.transform.position).magnitude <= Detection_distance)
        {
            Condition = Shark_Condition.Battle;
        }
        if ((player.transform.position - shark.transform.position).magnitude > Detection_distance)
        {
            Condition = Shark_Condition.Patrolling;
        }
    }
    void FixedUpdate()
    {
        switch (Condition)
        {
            case Shark_Condition.Patrolling://巡廻中
                for (int i = 0; i < pat_num; i++)
                {
                    if (current_pos_num == i)
                    {
                        LookAt2D_ob(Pat_pos_list[i]);
                    }
                }
                break;
            case Shark_Condition.Battle://追跡中
                LookAt2D_ob(player);
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("当たった");
        if (other.gameObject.CompareTag("Pat_pos"))
        {
            if (current_pos_num == (pat_num - 1)) current_pos_num = 0;
            else current_pos_num++;
        }
    }
    void LookAt2D_ob(GameObject target)
    {
        this.transform.LookAt(target.transform.position);
        shark.transform.localPosition += new Vector3(0, 0, move_speed);
        this.transform.position = shark.transform.position;
        shark.transform.localPosition = Vector3.zero;
    }
}