using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

class SignBoard : InformationUIBase
{
    [Header("オブジェクトの回転速度"), SerializeField]
    private float rotationSpeed = 5f;
    private List<Transform> nearPlayersList = new List<Transform>(); // 一番近い敵のTransform
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        var otherTrans = other.transform;
        var playerUI = PlayerInformationMaster.instance.playerSignBoardUIDic[otherTrans];        
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Down);
        
        base.OnTriggerEnter(other);

        if(nearPlayersList.Count == 0)
            StartCoroutine(RotateSignBoard());


        if(!nearPlayersList.Contains(otherTrans))
            nearPlayersList.Add(otherTrans);
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        var otherTrans = other.transform;
        var playerUI = PlayerInformationMaster.instance.playerSignBoardUIDic[otherTrans];
        playerUI.ResetMyParameters(SignBoardUIAnimatorCS.Parameters.Up);

        base.OnTriggerEnter(other);

        if (nearPlayersList.Contains(otherTrans))
            nearPlayersList.Remove(otherTrans);

        if (nearPlayersList.Count == 0)
            StopCoroutine(RotateSignBoard());
    }

    IEnumerator RotateSignBoard()
    {
        while (true)
        {
            yield return new WaitForSeconds(4 * Time.deltaTime); // 4フレーム待つ
            // オブジェクトを一番近い敵の方向に向ける
            Vector3 direction = nearPlayersList[0].position - transform.position;
            
            direction.y = 0; 

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void DisplayInformationUI(Transform otherT) 
    {
        ChangeText(otherT);
    }
    protected override void HideInformationUI(Transform otherT)
    {
        
    }
}