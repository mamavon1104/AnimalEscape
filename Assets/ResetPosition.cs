using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    [SerializeField] Transform resetPosition;

    public void ResetTransAll()
    {
        var myT = transform;
        myT.position = resetPosition.position;
        myT.rotation = resetPosition.rotation;
    }
}
