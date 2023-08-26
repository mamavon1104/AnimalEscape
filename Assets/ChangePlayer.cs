using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
class ChangePlayer : MonoBehaviour
{
    [Header("ÉvÉåÉCÉÑÅ[ÇÃInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _change;
    private int nowActivePlayer;

    private Transform[] playersTrans;
    private PlayerCS[] playersCs;

    private void Awake()
    {
        _change = action.currentActionMap["ChangePlayer"];
        _change.performed += ChangePlayerNum;
    }
    async void Start()
    {
        await UniTask.Delay(1);

        var obj = GameObject.FindGameObjectsWithTag("Player");
        playersTrans = new Transform[obj.Length];
        playersCs = new PlayerCS[obj.Length];

        for (int i = 0; i < obj.Length; i++)
        {
            Assert.IsTrue(obj[i].TryGetComponent<PlayerCS>(out var playerCS), "playerCSÇ™null");
            
            playersCs[i] = playerCS;
            playersTrans[i] = obj[i].transform;

            PlayerInformationMaster.instance.inputScriptDic[playersTrans[i]].Setting(false);
            playerCS.SetPlayerSelectionStatus(false);
        }

        PlayerInformationMaster.instance.inputScriptDic[playersTrans[nowActivePlayer]].Setting(true);
        playersCs[nowActivePlayer].SetPlayerSelectionStatus(true);
    }
    public void ChangePlayerNum(InputAction.CallbackContext context) 
    {
        nowActivePlayer = ++nowActivePlayer % playersCs.Length;
        for (int i = 0; i < playersCs.Length; i++)
        {
            bool thisBool;

            if (i == nowActivePlayer)
                thisBool = true;
            else
                thisBool = false;

            var inputCS = PlayerInformationMaster.instance.inputScriptDic[playersTrans[i]];
            inputCS.Setting(thisBool);
            playersCs[i].SetPlayerSelectionStatus(thisBool);
        }
    }
}
