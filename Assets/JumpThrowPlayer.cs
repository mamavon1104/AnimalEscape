using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpThrowPlayer : MonoBehaviour
{
    [Header("投げるポジション"), SerializeField]
    private Transform throwPointNow;
    [Header("Playerの値(PlayerValue)"), SerializeField]
    private MyPlayersValue playerValue;
    public void ThrowPlayerinJumpAction(Transform otherPlayer,PlayerCS playerCS)
    {
        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(otherPlayer.position, throwPointNow.position, playerValue.throwAngle);

        var otherRig = otherPlayer.GetComponent<Rigidbody>();
        otherRig.velocity = Vector3.zero;
        otherRig.AddForce(velocity, ForceMode.Impulse);
        
        playerCS.ChangeState(PlayerCS.PlayerState.BeingThrown);
    }
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        float rad = angle * Mathf.PI / 180;
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));
        float y = pointA.y - pointB.y;
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            return Vector3.zero;
        }
        else
        {
            // 到達地点までの速度ベクトル
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
