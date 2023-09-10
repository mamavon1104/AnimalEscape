using Cinemachine;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerInputScript : MonoBehaviour
{
    PlayerCS _playerCS;
    [Header("CatchObjectCS"), SerializeField]
    CatchPut_Items m_CatchObjectCS;
    [Header("ThrowToPointObj"), SerializeField]
    ThrowToPoint m_ThrowToPointCS;
    [Header("ThrowToPointObj"), SerializeField]
    GameUIImage m_GameUIImage;
    [Header("CinemachineVirtualCamera"), SerializeField]
    CinemachineVirtualCamera myCamera;
    [Header("ThrwoPointMove"),SerializeField]
    ThrowPointMove m_ThrowPointMove;
    CinemachinePOV cameraPOV;

    [Header("プレイヤーの値"), SerializeField]
    private MyPlayersValue playerValue;

    [Header("プレイヤーのInputActions"), SerializeField]
    private PlayerInput _action;
    private InputAction _move, _jump,_change, _throw, _catchPut, _pauseGame, _throwMove;
    private void Awake()
    {
        _playerCS = GetComponent<PlayerCS>();
        _action = transform.GetComponent<PlayerInput>();
        _change = _action.currentActionMap["ChangePlayer"];
        _move = _action.currentActionMap["Move"];    
        _jump = _action.currentActionMap["Jump"]; 
        _throw = _action.currentActionMap["ThrowObject"];
        _catchPut = _action.currentActionMap["CatchAndPut"];
        _pauseGame = _action.currentActionMap["PauseGame"];
        _throwMove = _action.currentActionMap["ThrowPositionMove"];
        cameraPOV = myCamera.GetCinemachineComponent<CinemachinePOV>();
    }

    public void Setting(bool setBool)
    {
        if (setBool)
        {
            _move.canceled += _playerCS.OnMove;
            _move.performed += _playerCS.OnMove;
            _jump.performed += _playerCS.OnJump;
            _pauseGame.performed += m_GameUIImage.PauseGame;
            _catchPut.performed += m_CatchObjectCS.CatchAndPut;
            _throw.performed += m_ThrowToPointCS.Select_OR_Throw;
            _change.performed += PlayerManager.Instance.ChangePlayerNum;                                                                                          
        }
        else
        {
            _move.canceled -= _playerCS.OnMove;
            _move.performed -= _playerCS.OnMove;
            _jump.performed -= _playerCS.OnJump;
            _pauseGame.performed -= m_GameUIImage.PauseGame;
            _catchPut.performed -= m_CatchObjectCS.CatchAndPut;
            _throw.performed -= m_ThrowToPointCS.Select_OR_Throw;
            _change.performed -= PlayerManager.Instance.ChangePlayerNum;
        }
        Debug.Log(setBool);
    }

    /// <summary>
    /// もしtrueなら動かし、falseなら動かさん。
    /// </summary>
    public void ChangeCameraMove(bool setBool)
    {
        if (setBool)
        {
            cameraPOV.m_VerticalAxis.m_MaxSpeed = playerValue.verticalCameraSpeed;
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = playerValue.horizontalCameraSpeed;
            _throwMove.performed += m_ThrowPointMove.OnMove;
            _throwMove.canceled += m_ThrowPointMove.OnMove;
        }
        else
        {
            cameraPOV.m_VerticalAxis.m_MaxSpeed = 0;
            cameraPOV.m_HorizontalAxis.m_MaxSpeed = 0;
            _throwMove.performed -= m_ThrowPointMove.OnMove;
            _throwMove.canceled -= m_ThrowPointMove.OnMove;
        }
    }
}