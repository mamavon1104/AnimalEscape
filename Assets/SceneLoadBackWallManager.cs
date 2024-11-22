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
        GetBackGround();
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            _childBlackWall[i].color = new Color(0, 0, 0, 1);
        }
        while (_childBlackWall[_childBlackWall.Length - 1].color.a >= 0)
        {
            await UniTask.Yield();
            for (int i = 0; i < _childBlackWall.Length; i++)
            {
                _childBlackWall[i].color -= BackGround_color_alpha;
            }
        }
        for (int i = 0; i < _childBlackWall.Length; i++)
        {
            _childBlackWall[i].enabled = false;
        }
    }
    private void GetBackGround()
    {
        _childBlackWall = new Image[1];
        _childBlackWall[0] = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }
}
