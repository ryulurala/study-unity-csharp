using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int _monsterCount = 0;
    [SerializeField] int _reserveCount = 0;
    [SerializeField] int _keepMonsterCount = 0;
    [SerializeField] Vector3 _spawnPos;
    [SerializeField] float _spawnRadius = 15.0f;
    [SerializeField] float _spawnTime = 5.0f;

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    void Start()
    {
        Manager.Game.OnSpawnEvent -= AddMonsterCount;
        Manager.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        while (_reserveCount + _monsterCount < _keepMonsterCount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        // 랜덤으로 0 ~ 5초 후

        // Monster Spawn
        GameObject obj = Manager.Game.Spawn(Define.WorldObject.Monster, "Skeleton");
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        // for. 유효한 path
        NavMeshPath path = new NavMeshPath();

        while (true)
        {
            // 랜덤 크기가 1인 구 형태 좌표 추출(for. 방향 벡터 추출)
            Vector3 randDir = Random.insideUnitSphere * _spawnRadius;
            randDir.y = 0;
            Vector3 randPos = _spawnPos + randDir;

            // 갈 수 있는 지
            if (nma.CalculatePath(randPos, path))
            {
                obj.transform.position = randPos;
                break;
            }
        }
        _reserveCount--;
    }
}
