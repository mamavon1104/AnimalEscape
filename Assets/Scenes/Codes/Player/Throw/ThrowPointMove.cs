using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowPointMove : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private LineRenderer lineRenderer;
    private Transform myT;

    private Vector2 _inputMove;　// 動く数値

    private void Awake()
    {
        myT = transform;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.4f;
    }
    private async void Update()
    {
        for (int i = 0; i < 4; i++)
             await UniTask.Yield();

        lineRenderer.SetPosition(0, myT.position);
        lineRenderer.SetPosition(1, player.position + Vector3.up);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値を保持しておく
        _inputMove = context.ReadValue<Vector2>();
    }
}
