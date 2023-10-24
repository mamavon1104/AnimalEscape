using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shark_move2 : MonoBehaviour
{
    public enum Shark_Condition
    {
        Patrolling,//����
        Battle,//�퓬
    }
    public Shark_Condition Condition;

    private GameObject player;
    private GameObject shark;

    private int pat_num;//����ꏊ�̐�
    private int current_pos_num = 0;//���݈ړ����Ă���ꏊ
    public float Detection_distance = 10;//�T�m�͈�
    public float move_speed = 0.08f;
    public List<GameObject> Pat_pos_list = new List<GameObject>();//����ʒu
    void Start()
    {
        Condition = Shark_Condition.Patrolling;
        //��Find����
        player = GameObject.Find("Player");
        shark = transform.Find("Shark").gameObject;

        //�����n�_�擾
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
        //Player�Ƃ̋����𑪂��Ēǂ��Ώ̂�ς���
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
            case Shark_Condition.Patrolling://������
                for (int i = 0; i < pat_num; i++)
                {
                    if (current_pos_num == i)
                    {
                        LookAt2D_ob(Pat_pos_list[i]);
                    }
                }
                break;
            case Shark_Condition.Battle://�ǐՒ�
                LookAt2D_ob(player);
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("��������");
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