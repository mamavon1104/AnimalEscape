using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;
class ChangePlayer : MonoBehaviour
{
    [Header("�v���C���[��InputActions"), SerializeField]
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
        await UniTask.Yield();

        var obj = GameObject.FindGameObjectsWithTag("Player");
        playersTrans = new Transform[obj.Length];
        playersCs = new PlayerCS[obj.Length];

        for (int i = 0; i < obj.Length; i++)
        {
            Assert.IsTrue(obj[i].TryGetComponent<PlayerCS>(out var playerCS), "playerCS��null");
            
            playersCs[i] = playerCS;
            playersTrans[i] = obj[i].transform;

            PlayerInformationManager.Instance.inputScriptDic[playersTrans[i]].Setting(false);
            playerCS.SetPlayerSelectionStatus(false);
        }

        Debug.Log(playersTrans[nowActivePlayer]);
        PlayerInformationManager.Instance.inputScriptDic[playersTrans[nowActivePlayer]].Setting(true);
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

            var inputCS = PlayerInformationManager.Instance.inputScriptDic[playersTrans[i]];
            inputCS.Setting(thisBool);
            playersCs[i].SetPlayerSelectionStatus(thisBool);
        }
    }
}
