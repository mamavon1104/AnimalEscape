using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadBackWallManager : ManagerSingletonBase<SceneLoadBackWallManager>
{
    [SerializeField] private Image[] _childBlackWall;
    private static Color BackGround_color_alpha = new Color(0, 0, 0, 0.005f);
    private async void Start()
    {
        GetBackGround();
        await UniTask.Yield();
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            _childBlackWall[i].enabled = false;
        }
    }
    public async UniTask FadeOut()
    {
        GetBackGround();
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            _childBlackWall[i].enabled = true;
            _childBlackWall[i].color = new Color(0, 0, 0, 0);
        }
        //最後の奴が終わるまで
        while (_childBlackWall[_childBlackWall.Length - 1].color.a <= 1)
        {
            await UniTask.Yield();
            for (int i = 0; i < _childBlackWall.Length; i++)
            {
                _childBlackWall[i].color += BackGround_color_alpha;
            }
        }
    }
    public async UniTask FadeIn()
    {
        Debug.Log("cccc");
        GetBackGround();
        Debug.Log(_childBlackWall.Length);
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            Debug.Log("cccc2");
            _childBlackWall[i].color = new Color(0, 0, 0, 1);
        }
        Debug.Log("c");
        while (_childBlackWall[_childBlackWall.Length - 1].color.a >= 0)
        {
            await UniTask.Yield();
            Debug.Log("b");
            for (int i = 0; i < _childBlackWall.Length; i++)
            {
                Debug.Log("a");
                _childBlackWall[i].color -= BackGround_color_alpha;
            }
        }
        Debug.Log("d");
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            Debug.Log("cccc3");
            _childBlackWall[i].enabled = false;
        }
    }
    private void GetBackGround()
    {
        var _backGroundObj = GameObject.FindGameObjectsWithTag("BackGround");
        _childBlackWall = new Image[_backGroundObj.Length];
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            _childBlackWall[i] = _backGroundObj[i].GetComponent<Image>();
        }
    }
}
