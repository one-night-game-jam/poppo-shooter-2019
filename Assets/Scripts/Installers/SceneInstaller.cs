using Characters.Enemies;
using Managers;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller<SceneInstaller>
{
    [SerializeField]
    EnemyCore enemyPrefab;

    public override void InstallBindings()
    {
        Container.BindIFactory<EnemyCore>()
            .FromComponentInNewPrefab(enemyPrefab);

        Container.Bind<EnemyContainer>().AsSingle();
    }
}