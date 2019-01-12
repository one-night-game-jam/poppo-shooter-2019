using System;
using System.Linq;
using Characters;
using Characters.Enemies;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        EnemyCore enemyPrefab;

        [SerializeField]
        double dueTimeSeconds;
        [SerializeField]
        double periodSeconds;

        [SerializeField]
        float randomSphereSize;

        [Inject]
        IFactory<EnemyCore> enemyFactory;

        [Inject]
        IReadOnlyPlayerCore player;

        [Inject]
        EnemyContainer container;

        void Start()
        {
            Observable.Timer(TimeSpan.FromSeconds(dueTimeSeconds), TimeSpan.FromSeconds(periodSeconds))
                .TakeUntil(player.Dead)
                .Select(_ => 1)
                .StartWith(0)
                .Scan(0, (a, b) => a + b)
                .Subscribe(CreateWave)
                .AddTo(this);
        }

        void CreateWave(int count)
        {
            foreach (var _ in Enumerable.Range(0, count))
            {
                container.Add(CreateEnemy());
            }
        }

        EnemyCore CreateEnemy()
        {
            var enemy = enemyFactory.Create();
            enemy.transform.position = Random.insideUnitSphere * randomSphereSize + transform.position;
            return enemy;
        }
    }
}
