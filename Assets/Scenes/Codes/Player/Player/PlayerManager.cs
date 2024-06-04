using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : ManagerSingletonBase<PlayerManager>
{
    private int nowActivePlayer = 0;

    [SerializeField] private Transform[] playersTrans;
    [SerializeField] private bool canChange = false;//連続して変えれないようにする。
    private PlayerCS[] playersCs;

    async void Start()
    {
        await UniTask.Yield(); // GameManagerSetting待機

        playersCs = new PlayerCS[playersTrans.Length];

        if (!GameValueManager.Instance.isPlayer2)
        {
            for (int i = 0; i < playersTrans.Length; i++)
            {
                var playerCS = playersTrans[i].GetComponent<PlayerCS>();
                playersCs[i] = playerCS;

                PlayerInformationManager.Instance.inputScriptDic[playersTrans[i]].Setting(false);
                playerCS.SetPlayerSelectionStatus(false);
                playersTrans[i].gameObject.SetActive(false);
            }
        }

        PlayerInformationManager.Instance.inputScriptDic[playersTrans[nowActivePlayer]].Setting(true);
        playersTrans[nowActivePlayer].gameObject.SetActive(true);
        playersCs[nowActivePlayer].SetPlayerSelectionStatus(true);

        if (GameValueManager.Instance.isPlayer2)
            this.enabled = false;
    }
    public void ChangePlayerNum(InputAction.CallbackContext context)
    {
        if (!canChange)
            return;

        canChange = false;
        SetCanChangeVariable();

        nowActivePlayer = ++nowActivePlayer % playersCs.Length;
        for (int i = 0; i < playersCs.Length; i++)
        {
            bool thisBool;

            if (i == nowActivePlayer)
                thisBool = true;
            else
                thisBool = false;

            var inputCS = PlayerInformationManager.Instance.inputScriptDic[playersTrans[i]];
            inputCS.Setting(thisBool);
            playersCs[i].SetPlayerSelectionStatus(thisBool);
        }
    }

    private async void SetCanChangeVariable()
    {
        await UniTask.Delay(100);
        canChange = true;
    }
    public void CanBeChange() => canChange = true;
}
