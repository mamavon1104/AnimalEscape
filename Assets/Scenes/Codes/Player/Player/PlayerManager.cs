using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
public class PlayerManager : ManagerSingletonBase<PlayerManager>
{
    private int nowActivePlayer = 0;

    private Transform[] playersTrans;
    private PlayerCS[] playersCs;
    private bool canChange = false;//òAë±ÇµÇƒïœÇ¶ÇÍÇ»Ç¢ÇÊÇ§Ç…Ç∑ÇÈÅB

    async void Start()
    {
        await UniTask.Yield(); // GameManagerSettingë“ã@

        var obj = GameObject.FindGameObjectsWithTag("Player");
        
        playersTrans = new Transform[obj.Length];
        playersCs = new PlayerCS[obj.Length];

        if (!GameValueManager.Instance.isPlayer2)
            NoMultiGameSetting(obj);
        
        PlayerInformationManager.Instance.inputScriptDic[playersTrans[nowActivePlayer]].Setting(true);
        playersCs[nowActivePlayer].SetPlayerSelectionStatus(true);
        obj[nowActivePlayer].SetActive(true);
        
        if (GameValueManager.Instance.isPlayer2)
            this.enabled = false;
    }
    private void NoMultiGameSetting(GameObject[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            Assert.IsTrue(obj[i].TryGetComponent<PlayerCS>(out var playerCS), "playerCSÇ™null");

            playersCs[i] = playerCS;
            playersTrans[i] = obj[i].transform;

            PlayerInformationManager.Instance.inputScriptDic[playersTrans[i]].Setting(false);
            playerCS.SetPlayerSelectionStatus(false);
            obj[i].SetActive(false);
        }
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
