using UnityEngine;
using System.Collections.Generic;
using TMPro;

// Author : Auguste Paccapelo

public class MoveComponent : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private PhysicsMoveSettings _physicsMoveSettings;

    private IController _controller;
    private Rigidbody _rb;

    // ----- Others ----- \\

    private Vector3 _inputDir = Vector3.zero;

    private bool _isCrouched;

    [Header("Jump Settings")]
    [SerializeField] private LayerMask _floorLayers = 0;
    [SerializeField] private Vector3 _footsPos = Vector3.zero;

    private List<GameObject> _floorContacts = new();
    private bool _isGrounded => _floorContacts.Count != 0;
    public bool IsGrounded => _isGrounded;

    private int _currentJumpDone = 0;

    [Header("Debug")]
    [SerializeField] private bool _drawGizmos = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        _controller = GetComponentInChildren<IController>();
        _rb = GetComponentInChildren<Rigidbody>();
        
        ConnectControllerEvents();
    }

    private void OnDisable()
    {
        DisconnectControllerEvents();

        _controller = null;
        _rb = null;
    }

    private void Awake() { }

    private void Start() { }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject.layer, _floorLayers))
        {
            FloorCollisionEnter(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
        if (IsInLayerMask(collision.gameObject.layer, _floorLayers))
        {
            FloorCollisionExit(collision);
        }
    }

    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        Gizmos.DrawSphere(transform.position + _footsPos, 0.1f);
    }

    // ----- My Functions ----- \\

    private void FloorCollisionEnter(Collision coll)
    {
        bool isGroundBelow = coll.contacts[0].point.y <= _footsPos.y;

        if (isGroundBelow)
        {
            _floorContacts.Add(coll.gameObject);
            _currentJumpDone = 0;
        }
    }

    private void FloorCollisionExit(Collision coll)
    {
        if (_floorContacts.Contains(coll.gameObject))
        {
            _floorContacts.Remove(coll.gameObject);
        }
    }

    private void Move()
    {
        if (_rb == null) return;

        float speed;
        if (_isCrouched)
        {
            speed = _physicsMoveSettings.crouchSpeed;
        }
        else
        {
            speed = _physicsMoveSettings.walkSpeed;
        }

        Vector3 direction = _inputDir.z * transform.forward + _inputDir.x * transform.right;

        _rb.AddForce(direction * speed * Time.deltaTime, ForceMode.Acceleration);
    }

    private void OnMove(Vector2 obj)
    {
        _inputDir = new Vector3(obj.x, 0, obj.y);
    }

    private void OnJump()
    {
        if (_rb == null) return;
        if (!CanJump()) return;

        float force = _physicsMoveSettings.GetJumpForce(_currentJumpDone);

        _rb.AddForce(Vector3.up * force, ForceMode.Impulse);

        _currentJumpDone++;
    }

    private bool CanJump()
    {
        return _isGrounded || CanAirJump();
    }

    private bool CanAirJump()
    {
        if (!_physicsMoveSettings.canMultiJump) return false;

        return _currentJumpDone < _physicsMoveSettings.numJumps;
    }

    private void OnCrouch()
    {
        _isCrouched = true;
    }

    private void OnUncrouch()
    {
        _isCrouched = false;
    }

    private void ConnectControllerEvents()
    {
        if (_controller == null) return;

        ((IMoveController)_controller).onMoveValue += OnMove;

        if (_physicsMoveSettings.canJump) ((IJumpController)_controller).onJumpStart += OnJump;
        if (_physicsMoveSettings.canCrouch)
        {
            ((ICrouchController)_controller).onCrouchStart += OnCrouch;
            ((ICrouchController)_controller).onCrouchEnd += OnUncrouch;
        }
    }

    private void DisconnectControllerEvents()
    {
        if (_controller == null) return;

        ((IMoveController)_controller).onMoveValue -= OnMove;

        if (_physicsMoveSettings.canJump) ((IJumpController)_controller).onJumpStart -= OnJump;
        if (_physicsMoveSettings.canCrouch)
        {
            ((ICrouchController)_controller).onCrouchStart -= OnCrouch;
            ((ICrouchController)_controller).onCrouchEnd -= OnUncrouch;
        }
    }

    private bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}