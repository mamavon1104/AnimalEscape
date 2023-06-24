using UnityEngine;
using UnityEngine.InputSystem;

public class ChangePlayer : MonoBehaviour
{
    [Header("�v���C���[�����A�擾�o�����炱���ɕ\������"),SerializeField]
    private PlayerCs[] playersCs;
    private int nowActivePlayer;

    private PlayerInput playerInput;
    private InputAction playerChange;

    private void Start()
    {
        playerInput = transform.GetComponent<PlayerInput>();
        playerChange = playerInput.currentActionMap["ChangePlayer"];
        playerChange.performed += ChangePlayerNum; 

        var obj = GameObject.FindGameObjectsWithTag("Player");
        playersCs = new PlayerCs[obj.Length];

        Debug.Log("Plyaer�̃^�O���t����object : " + obj.Length);

        for (int i = 0; i < obj.Length; i++)
        {
            var playerCS = obj[i].GetComponent<PlayerCs>();
            if (playerCS != null)
                playersCs[i] = playerCS;
        }
        playersCs[nowActivePlayer].SetPlayerSelectionStatus(true);
    }
    void ChangePlayerNum(InputAction.CallbackContext context) 
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
