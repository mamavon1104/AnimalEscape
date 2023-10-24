using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndPressButtonCS : MonoBehaviour
{
    [Header("�v���C���[��InputActions"), SerializeField]
    private PlayerInput _action;
    private InputAction _catchPut; 
    void Start()
    {
        _catchPut = _action.currentActionMap["CatchAndPut"];
        _catchPut.performed += RoadScene;
    }
    private void RoadScene(InputAction.CallbackContext _) => GetComponent<ChangeSceneMaster>().RoadScene();
}
