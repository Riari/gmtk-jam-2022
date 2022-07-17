using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [field: SerializeField]
    public float ZoomSpeed = 0.2f;

    [field: SerializeField]
    public float SmoothSpeed = 6.0f;

    [field: SerializeField]
    public float PanSpeed = 8.0f;

    [field: SerializeField]
    public float AutoPanSpeed = 25.0f;

    private const float MinSize = 3.0f;
    private const float MaxSize = 6.0f;

    private Camera _camera;
    private float _targetSize;

    readonly private Vector3 _defaultFocalPoint = new Vector3(0, 0, -10);
    private Vector3 _focalPoint;

    private bool _panLeft;
    private bool _panRight;
    private bool _panUp;
    private bool _panDown;

    void Start()
    {
        _camera = GetComponent<Camera>();
        _targetSize = _camera.orthographicSize;
        _focalPoint = _defaultFocalPoint;
    }

    void Update()
    {
        if (_targetSize != _camera.orthographicSize)
        {
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _targetSize, SmoothSpeed * Time.deltaTime);
        }

        if (_focalPoint != _defaultFocalPoint)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _focalPoint, AutoPanSpeed * Time.deltaTime);

            if (_focalPoint == _camera.transform.position)
            {
                _focalPoint = _defaultFocalPoint;
            }

            return;
        }

        Vector3 pan = Vector3.zero;
        if (_panLeft) pan.x -= PanSpeed;
        if (_panRight) pan.x += PanSpeed;
        if (_panDown) pan.y -= PanSpeed;
        if (_panUp) pan.y += PanSpeed;

        _camera.transform.position += pan * Time.deltaTime;
    }

    public void FocusOn(GameObject obj)
    {
        _focalPoint.x = obj.transform.position.x;
        _focalPoint.y = obj.transform.position.y;
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        _panLeft = context.performed && !context.canceled;
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        _panRight = context.performed && !context.canceled;
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        _panUp = context.performed && !context.canceled;
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        _panDown = context.performed && !context.canceled;
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
