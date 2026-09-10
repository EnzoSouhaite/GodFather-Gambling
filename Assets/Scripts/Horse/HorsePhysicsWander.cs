using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class HorsePhysicsWander : MonoBehaviour
{
    [Header("Vitesse")]
    [SerializeField] float _speed = 3f;
 
    [Header("Tendance vers l'arrivée")]
    [Tooltip("Point vers lequel les chevaux sont globalement attirés (laisser vide = mouvement 100% aléatoire, comme dans l'enclos d'attente)")]
    [SerializeField] Transform _finishTarget;
    [Range(0f, 1f)]
    [Tooltip("0 = mouvement 100% aléatoire (enclos d'attente), 1 = va tout droit vers l'arrivée sans aléatoire. Une valeur autour de 0.3-0.4 donne un bon compromis.")]
    [SerializeField] float _finishBias = 0f;
 
    [Header("Changement de direction")]
    [Tooltip("Si coché, le cheval ne change de direction QUE lorsqu'il touche un mur/obstacle. " +
             "Si décoché, il change aussi spontanément à intervalle régulier (peut sembler 'tourner dans le vide').")]
    [SerializeField] bool _onlyChangeOnCollision = true;
    [Tooltip("Intervalle moyen entre deux changements de direction volontaires (ignoré si 'Only Change On Collision' est coché)")]
    [SerializeField] float _directionChangeInterval = 1.5f;
    [Tooltip("Variation aléatoire de cet intervalle (+/-)")]
    [SerializeField] float _intervalVariance = 0.5f;
 
    [Header("Rebond sur collision")]
    [Tooltip("Angle aléatoire ajouté après un rebond sur un mur (en degrés), pour éviter les rebonds trop mécaniques")]
    [SerializeField] float _bounceRandomAngle = 25f;
 
    Rigidbody2D _rb;
    Coroutine _directionRoutine;
    bool _isWandering = false;
 
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
        _rb.freezeRotation = true;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        
        _rb.linearVelocity = Vector2.zero;
        _isWandering = false;
    }
 
    public void StartWandering()
    {
        _isWandering = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
        SetRandomVelocity();
 
        if (_directionRoutine != null) StopCoroutine(_directionRoutine);
 
        if (!_onlyChangeOnCollision)
        {
            _directionRoutine = StartCoroutine(DirectionChangeRoutine());
        }
    }
 
    public void StopWandering()
    {
        _isWandering = false;
        if (_directionRoutine != null) StopCoroutine(_directionRoutine);
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.linearVelocity = Vector2.zero;
    }
    public void ConfigureMovement(float speed, Transform finishTarget = null, float finishBias = 0f)
    {
        _speed = speed;
        _finishTarget = finishTarget;
        _finishBias = finishBias;
 
        if (_isWandering)
        {
            SetRandomVelocity();
        }
    }
 
    IEnumerator DirectionChangeRoutine()
    {
        while (true)
        {
            float wait = Random.Range(
                _directionChangeInterval - _intervalVariance,
                _directionChangeInterval + _intervalVariance
            );
            yield return new WaitForSeconds(Mathf.Max(0.1f, wait));
 
            SetRandomVelocity();
        }
    }
 
    void SetRandomVelocity()
    {
        Vector2 direction;
 
        if (_finishTarget != null && _finishBias > 0f)
        {
            Vector2 towardFinish = ((Vector2)_finishTarget.position - _rb.position).normalized;
 
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 randomDir = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
 
            direction = Vector2.Lerp(randomDir, towardFinish, _finishBias).normalized;
        }
        else
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
 
        _rb.linearVelocity = direction * _speed;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 currentVelocity = _rb.linearVelocity;
        if (currentVelocity.sqrMagnitude < 0.01f) return;
 
        float randomOffset = Random.Range(-_bounceRandomAngle, _bounceRandomAngle);
        Vector2 rotatedVelocity = Quaternion.Euler(0, 0, randomOffset) * currentVelocity;
 
        _rb.linearVelocity = rotatedVelocity.normalized * _speed;
    }
}