using UnityEngine;
using UnityEngine.InputSystem;
class ChangePlayer : MonoBehaviour
{

    [Header("ÉvÉåÉCÉÑÅ[ÇÃInputActions"), SerializeField]
    private PlayerInput action;
    private InputAction _change;

    private int nowActivePlayer;
    private PlayerCS[] playersCs;

    private void Awake()
    {
        _change = action.currentActionMap["ChangePlayer"];
        _change.performed += ChangePlayerNum;
    }
    private void Start()
    {
        var obj = GameObject.FindGameObjectsWithTag("Player");
        playersCs = new PlayerCS[obj.Length];

        for (int i = 0; i < obj.Length; i++)
        {
            var playerCS = obj[i].GetComponent<PlayerCS>();
            if (playerCS != null)
            {
                playersCs[i] = playerCS;
                playerCS.SetPlayerSelectionStatus(false);
            }
        }
        playersCs[nowActivePlayer].SetPlayerSelectionStatus(true);
    }
    public void ChangePlayerNum(InputAction.CallbackContext context) 
    {
        nowActivePlayer = ++nowActivePlayer % playersCs.Length;
        for (int i = 0; i < playersCs.Length; i++)
        {
            if (i == nowActivePlayer)
                playersCs[i].SetPlayerSelectionStatus(true);
            else
                playersCs[i].SetPlayerSelectionStatus(false);
        }
    }
}
