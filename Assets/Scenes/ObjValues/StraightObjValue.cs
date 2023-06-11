using UnityEngine;

[CreateAssetMenu(menuName = "ObjValue/StraightObjValue")]
public class StraightObjValue : ObjectsValueBase
{
    public enum GoToDirections
    {
        [InspectorName("x�̕�����")]
        x,
        [InspectorName("y�̕�����")]
        y,
        [InspectorName("z�̕�����")]
        z
    }

    public enum StopPos
    {
        [InspectorName("�[�Ŏ~�܂�")]
        stop,
        [InspectorName("�~�܂�Ȃ�")]
        dontStop
    }

    [Header("Obj���l����A�������ׂ������́H")]
    public GoToDirections directions;
    [Header("�����Ŏ~�܂�܂��H")]
    public StopPos stopPos;
}
