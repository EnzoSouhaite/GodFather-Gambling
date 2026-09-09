using System;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    [SerializeField] private float sens = 0.25f;
    private IMouseXYAxisController mouseController;

    private void Start()
    {
        GameManager.Instance.HideMouse();
    }

    private void OnEnable()
    {
        mouseController = mouseController ?? GetComponent<IMouseXYAxisController>();

        if (mouseController == null) return;

        mouseController.onMouseValue += OnMouseMove;
    }

    private void OnDisable()
    {
        if (mouseController == null) return;

        mouseController.onMouseValue -= OnMouseMove;
    }

    float _pitch = 0f;
    private void OnMouseMove(Vector2 obj)
    {
        Vector3 eular = transform.eulerAngles;
        eular.y += obj.x * sens;
        transform.eulerAngles = eular;

        _pitch -= obj.y * sens;
        _pitch = Mathf.Clamp(_pitch, -90, 90);
        _camera.localEulerAngles = new Vector3(_pitch, 0, 0);
    }
}