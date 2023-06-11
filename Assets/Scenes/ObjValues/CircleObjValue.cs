using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ObjValue/CircleObjValue")]
public class CircleObjValue : ObjectsValueBase
{
    [Header("obj�̋O���̕ϐ�")]
    public int resolution = 12;

    public enum StopPos
    {
        [InspectorName("�㉺�Ŏ~�܂�")]
        stopVertical,
        [InspectorName("���E�Ŏ~�܂�")]
        stopHorizontal,
        [InspectorName("�~�܂�Ȃ�")]
        dontStop
    }

    [Header("�����Ŏ~�܂�܂��H")]
    public StopPos stopPos;
}
