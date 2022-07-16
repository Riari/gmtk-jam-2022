using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [field: SerializeField]
    public float ZoomSpeed = 0.2f;

    [field: SerializeField]
    public float SmoothSpeed = 6.0f;

    [field: SerializeField]
    public float PanSpeed = 5.0f;

    private const float MinSize = 3.0f;
    private const float MaxSize = 6.0f;

    private Camera _camera;
    private float _targetSize;

    private Vector3 _pan = Vector3.zero;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _targetSize = _camera.orthographicSize;
    }

    void Update()
    {
        if (_targetSize != _camera.orthographicSize)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _targetSize, SmoothSpeed * Time.deltaTime);
        }

        _camera.transform.position += _pan * Time.deltaTime;
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.performed && !context.canceled) _pan.x = -PanSpeed;
        else _pan.x = 0;
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (context.performed && !context.canceled) _pan.x = PanSpeed;
        else _pan.x = 0;
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        if (context.performed && !context.canceled) _pan.y = PanSpeed;
        else _pan.y = 0;
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        if (context.performed && !context.canceled) _pan.y = -PanSpeed;
        else _pan.y = 0;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        var value = context.action.ReadValue<float>();

        if (value == 0) return;

        if (value > 0)
        {
            _targetSize -= ZoomSpeed;
        }
        else
        {
            _targetSize += ZoomSpeed;
        }

        _targetSize = Mathf.Clamp(_targetSize, MinSize, MaxSize);
    }
}
