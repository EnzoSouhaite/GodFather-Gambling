using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HorseWander : MonoBehaviour
{
    float _speed;
    float _pauseDuration;
    float _padding;
 
    Vector2 _zoneCenter;
    Vector2 _zoneSize;
 
    Vector2 _targetPosition;
    Coroutine _wanderRoutine;
    
    public void Setup(Vector2 zoneCenter, Vector2 zoneSize, float speed, float pauseDuration, float padding = 0.5f, float startDelay = 0f)
    {
        _zoneCenter = zoneCenter;
        _zoneSize = zoneSize;
        _speed = speed;
        _pauseDuration = pauseDuration;
        _padding = padding;
 
        if (_wanderRoutine != null) StopCoroutine(_wanderRoutine);
        _wanderRoutine = StartCoroutine(WanderRoutine(startDelay));
    }
 
    IEnumerator WanderRoutine(float startDelay)
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSeconds(startDelay);
        }
 
        while (true)
        {
            _targetPosition = GetRandomPositionInZone();
 
            yield return StartCoroutine(MoveToTarget(_targetPosition));
 
            float pause = Random.Range(_pauseDuration * 0.5f, _pauseDuration * 1.5f);
            yield return new WaitForSeconds(pause);
        }
    }
 
    Vector2 GetRandomPositionInZone()
    {
        float halfWidth = _zoneSize.x / 2f - _padding;
        float halfHeight = _zoneSize.y / 2f - _padding;
 
        float x = Random.Range(_zoneCenter.x - halfWidth, _zoneCenter.x + halfWidth);
        float y = Random.Range(_zoneCenter.y - halfHeight, _zoneCenter.y + halfHeight);
 
        return new Vector2(x, y);
    }
 
    IEnumerator MoveToTarget(Vector2 target)
    {
        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                _speed * Time.deltaTime
            );
 
            yield return null;
        }
 
        transform.position = target;
    }
 
    void OnDestroy()
    {
        if (_wanderRoutine != null) StopCoroutine(_wanderRoutine);
        if (_raceRoutine != null) StopCoroutine(_raceRoutine);
    }
    
    Coroutine _raceRoutine;
    
    [Header("Avancée pendant la course")]
    [Tooltip("Progression min/max faite d'un bond à l'autre le long du segment (0-1)")]
    [SerializeField] float _minStepProgress = 0.15f;
    [SerializeField] float _maxStepProgress = 0.35f;
    [Tooltip("Pause entre deux bonds pendant la course (0 = aucune pause, mouvement continu)")]
    [SerializeField] float _raceStepPause = 0.05f;
    
    public void StartRace(RacePath path, float raceSpeed, System.Action onFinished = null)
    {
        if (_wanderRoutine != null) StopCoroutine(_wanderRoutine);
        if (_raceRoutine != null) StopCoroutine(_raceRoutine);
 
        _speed = raceSpeed;
        _raceRoutine = StartCoroutine(RaceRoutine(path, onFinished));
    }

    IEnumerator RaceRoutine(RacePath path, System.Action onFinished)
    {
        if (path == null || path.waypoints == null || path.waypoints.Length < 2)
        {
            Debug.LogWarning("[HorseWander] RacePath invalide (moins de 2 waypoints).");
            onFinished?.Invoke();
            yield break;
        }

        for (int i = 0; i < path.waypoints.Length - 1; i++)
        {
            Vector2 segStart = path.waypoints[i].position;
            Vector2 segEnd = path.waypoints[i + 1].position;

            Vector2 segDir = (segEnd - segStart).normalized;
            Vector2 perpendicular = new Vector2(-segDir.y, segDir.x);

            float progress = 0f;

            while (progress < 1f)
            {
                progress = Mathf.Clamp01(progress + Random.Range(_minStepProgress, _maxStepProgress));

                // Décalage latéral aléatoire dans la largeur du couloir : c'est ce qui donne
                // l'effet de "rebond" d'un bord à l'autre plutôt qu'une trajectoire rectiligne.
                // La largeur peut varier le long du chemin (couloir plus étroit à certains endroits).
                float currentWidth = path.GetWidthAt(i, progress);
                float lateralOffset = Random.Range(-currentWidth / 2f, currentWidth / 2f);

                Vector2 pointOnLine = Vector2.Lerp(segStart, segEnd, progress);
                Vector2 target = pointOnLine + perpendicular * lateralOffset;

                yield return StartCoroutine(MoveToTarget(target));

                if (_raceStepPause > 0f)
                {
                    yield return new WaitForSeconds(Random.Range(_raceStepPause * 0.5f, _raceStepPause * 1.5f));
                }
            }
        }
        transform.position = path.waypoints[path.waypoints.Length - 1].position;
        onFinished?.Invoke();
    }
}
