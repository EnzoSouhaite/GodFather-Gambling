using UnityEngine;

public class HorseSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] GameObject[] _horsePrefabs;
 
    [Header("Nombre de chevaux à spawn")]
    [Range(1, 50)]
    [SerializeField] int _horseCount = 6;
    
    [Header("Zone de spawn / d'attente (haut-gauche)")]
    [Tooltip("Centre de la zone où les chevaux apparaissent et patientent avant la course")]
    [SerializeField] Vector2 _zoneCenter = Vector2.zero;
    [Tooltip("Taille de la zone d'attente")]
    [SerializeField] Vector2 _zoneSize = new Vector2(3f, 2f);
 
    [Header("Disposition du spawn")]
    [Tooltip("Si coché, les chevaux apparaissent rangés en grille (comme sur votre image de référence). " +
             "Si décoché, ils apparaissent à des positions aléatoires dans la zone.")]
    [SerializeField] bool _useGridLayout = true;
    [Tooltip("Nombre de colonnes de la grille")]
    [SerializeField] int _gridColumns = 4;
    [SerializeField] float _gridSpacing = 0.3f;
 
    [Header("Arrivée (bas-droite)")]
    [Tooltip("Transform positionné sur la ligne/zone d'arrivée")]
    [SerializeField] Transform _finishPoint;
    [SerializeField] FinishLine _finishLine;
    [Range(0f, 1f)]
    [Tooltip("Force de la tendance vers l'arrivée pendant la course (0 = aléatoire pur, 1 = ligne droite)")]
    [SerializeField] float _finishBias = 0.35f;
 
    [Header("Mouvement")]
    [Tooltip("Vitesse pendant l'attente dans l'enclos")]
    [SerializeField] float _waitSpeed = 1.5f;
    [Tooltip("Vitesse pendant la course")]
    [SerializeField] float _raceSpeed = 3f;
    [Tooltip("Temps d'attente avant que les chevaux commencent à bouger dans l'enclos")]
    [SerializeField] float _startDelay = 3f;
    
    [Header("Debug / Test")]
    [Tooltip("Si coché, lance automatiquement le spawn au démarrage de la scène")]
    [SerializeField] bool _autoSpawnOnStart = true;
 
    readonly System.Collections.Generic.List<HorsePhysicsWander> _spawnedHorses = new();
 
    void Start()
    {
        if (_autoSpawnOnStart)
        {
            SpawnHorses();
        }
    }
    
    public void SpawnHorses()
    {
        _spawnedHorses.Clear();
 
        if (_finishLine != null) _finishLine.ResetRace();
 
        if (_horsePrefabs == null || _horsePrefabs.Length == 0)
        {
            Debug.LogWarning("[HorseSpawner] Aucun prefab dans _horsePrefabs.");
            return;
        }
        
        System.Collections.Generic.List<GameObject> skinPool = GetShuffledSkinPool();
        
        for (int i = 0; i < _horseCount; i++)
        {
            Vector2 spawnPos = _useGridLayout ? GetGridPosition(i) : GetRandomPositionInZone();
 
            if (skinPool.Count == 0) skinPool = GetShuffledSkinPool();
            GameObject prefabToUse = skinPool[0];
            skinPool.RemoveAt(0);
 
            GameObject horse = Instantiate(prefabToUse, spawnPos, Quaternion.identity);
 
            HorsePhysicsWander wander = horse.GetComponent<HorsePhysicsWander>();
            if (wander == null) wander = horse.AddComponent<HorsePhysicsWander>();
 
            wander.ConfigureMovement(_waitSpeed, finishTarget: null, finishBias: 0f);
 
            _spawnedHorses.Add(wander);
        }
 
        Invoke(nameof(BeginWaitingWander), _startDelay);
    }

    System.Collections.Generic.List<GameObject> GetShuffledSkinPool()
    {
        var pool = new System.Collections.Generic.List<GameObject>(_horsePrefabs);

        for (int i = pool.Count - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }
        return pool;
    }
    
    void BeginWaitingWander()
    {
        foreach (HorsePhysicsWander horse in _spawnedHorses)
        {
            if (horse != null) horse.StartWandering();
        }
    }
    
    public void StartRace()
    {
        if (_finishPoint == null)
        {
            Debug.LogWarning("[HorseSpawner] Aucun _finishPoint assigné dans l'inspecteur.");
            return;
        }
 
        foreach (HorsePhysicsWander horse in _spawnedHorses)
        {
            if (horse == null) continue;
 
            horse.ConfigureMovement(_raceSpeed, _finishPoint, _finishBias);
        }
 
        if (_finishLine != null)
        {
            _finishLine.OnHorseFinished += HandleHorseFinished;
        }
    }
 
    void HandleHorseFinished(GameObject horse, int rank)
    {
        Debug.Log($"[HorseSpawner] {horse.name} termine {rank}{(rank == 1 ? "er" : "ème")}");
        //brancher ici UI de résultats / tiercé
    }
 
    Vector2 GetRandomPositionInZone()
    {
        float halfWidth = _zoneSize.x / 2f;
        float halfHeight = _zoneSize.y / 2f;
 
        float x = Random.Range(_zoneCenter.x - halfWidth, _zoneCenter.x + halfWidth);
        float y = Random.Range(_zoneCenter.y - halfHeight, _zoneCenter.y + halfHeight);
 
        return new Vector2(x, y);
    }
 
    Vector2 GetGridPosition(int index)
    {
        int columns = Mathf.Max(1, _gridColumns);
        int rows = Mathf.CeilToInt((float)_horseCount / columns);
 
        int col = index % columns;
        int row = index / columns;
        
        float totalSpacingX = _gridSpacing * (columns - 1);
        float totalSpacingY = _gridSpacing * (rows - 1);
 
        float cellWidth = (_zoneSize.x - totalSpacingX) / columns;
        float cellHeight = (_zoneSize.y - totalSpacingY) / rows;
 
        float startX = _zoneCenter.x - _zoneSize.x / 2f;
        float startY = _zoneCenter.y + _zoneSize.y / 2f;
 
        float x = startX + col * (cellWidth + _gridSpacing) + cellWidth / 2f;
        float y = startY - row * (cellHeight + _gridSpacing) - cellHeight / 2f;
 
        return new Vector2(x, y);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_zoneCenter, _zoneSize);
 
        if (_finishPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_finishPoint.position, 0.5f);
        }
    }
 
    void OnDestroy()
    {
        if (_finishLine != null)
        {
            _finishLine.OnHorseFinished -= HandleHorseFinished;
        }
    }
}

