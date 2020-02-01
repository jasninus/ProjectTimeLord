using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : ObjectPool<EnemySpawner, Enemy, Vector2>, IDataPersister
{
    public int totalEnemiesToBeSpawned;
    public int concurrentEnemiesToBeSpawned;
    public float spawnArea = 1.0f;
    public float spawnDelay;
    public float removalDelay;
    public DataSettings dataSettings;

    protected int totalSpawnedEnemyCount;
    protected int currentSpawnedEnemyCount;
    protected Coroutine spawnTimerCoroutine;
    protected WaitForSeconds spawnWait;

    void OnEnable()
    {
        PersistentDataManager.RegisterPersister(this);
    }

    void OnDisable()
    {
        PersistentDataManager.UnregisterPersister(this);
    }

    void Start()
    {
        for (int i = 0; i < initialPoolCount; i++)
        {
            Enemy newEnemy = CreateNewPoolObject();
            pool.Add(newEnemy);
        }

        int spawnCount = Mathf.Min(totalEnemiesToBeSpawned - totalSpawnedEnemyCount, concurrentEnemiesToBeSpawned);

        for (int i = 0; i < spawnCount; i++)
        {
            Pop(transform.position + transform.right * Random.Range(-spawnArea * 0.5f, spawnArea * 0.5f));
        }

        currentSpawnedEnemyCount = spawnCount;
        totalSpawnedEnemyCount += concurrentEnemiesToBeSpawned;
        spawnWait = new WaitForSeconds(spawnDelay);
    }

    public override void Push(Enemy poolObject)
    {
        poolObject.inPool = true;
        currentSpawnedEnemyCount--;
        poolObject.Sleep();
        StartSpawnTimer();
    }

    protected void StartSpawnTimer()
    {
        if (spawnTimerCoroutine == null)
            spawnTimerCoroutine = StartCoroutine(SpawnTimer());
    }

    protected IEnumerator SpawnTimer()
    {
        while (currentSpawnedEnemyCount < concurrentEnemiesToBeSpawned && totalSpawnedEnemyCount < totalEnemiesToBeSpawned)
        {
            yield return spawnWait;
            Pop(transform.position);
            currentSpawnedEnemyCount++;
            totalSpawnedEnemyCount++;
        }

        spawnTimerCoroutine = null;
    }

    public DataSettings GetDataSettings()
    {
        return dataSettings;
    }

    public void SetDataSettings(string dataTag, DataSettings.PersistenceType persistenceType)
    {
        dataSettings.dataTag = dataTag;
        dataSettings.persistenceType = persistenceType;
    }

    public Data SaveData()
    {
        return new Data<int>(totalSpawnedEnemyCount);
    }

    public void LoadData(Data data)
    {
        Data<int> enemyData = (Data<int>)data;
        totalSpawnedEnemyCount = enemyData.value;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea, 0.4f, 0));
    }
}

public class Enemy : PoolObject<EnemySpawner, Enemy, Vector2>
{
    public Damageable damageable;
    public EnemyBehaviour enemyBehaviour;

    protected WaitForSeconds m_RemoveWait;

    protected override void SetReferences()
    {
        damageable = instance.GetComponent<Damageable>();
        enemyBehaviour = instance.GetComponent<EnemyBehaviour>();

        damageable.OnDie.AddListener(ReturnToPoolEvent);

        m_RemoveWait = new WaitForSeconds(objectPool.removalDelay);
    }

    public override void WakeUp(Vector2 info)
    {
        enemyBehaviour.SetMoveVector(Vector2.zero);
        instance.transform.position = info;
        instance.SetActive(true);
        damageable.SetHealth(damageable.startingHealth);
        damageable.DisableInvulnerability();
        enemyBehaviour.contactDamager.EnableDamage();
        SceneLinkedSMB<EnemyBehaviour>.Initialise(enemyBehaviour.GetComponent<Animator>(), enemyBehaviour);
        enemyBehaviour.EndAttack();
    }

    public override void Sleep()
    {
        instance.SetActive(false);
        damageable.EnableInvulnerability();
    }

    protected void ReturnToPoolEvent(Damager dmgr, Damageable dmgbl)
    {
        objectPool.StartCoroutine(ReturnToPoolAfterDelay());
    }

    protected IEnumerator ReturnToPoolAfterDelay()
    {
        yield return m_RemoveWait;
        ReturnToPool();
    }
}
