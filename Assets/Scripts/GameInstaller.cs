using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public GameObject playerPrefab; 
    public GameObject enemyManagerPrefab; 
    public GameObject lootManagerPrefab;
    public GameObject upgradeManagerPrefab; 
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromComponentInNewPrefab(playerPrefab).AsSingle();
        Container.Bind<EnemyManager>().FromComponentInNewPrefab(enemyManagerPrefab).AsSingle();
        Container.Bind<LootManager>().FromComponentInNewPrefab(lootManagerPrefab).AsSingle();
        Container.Bind<UpgradeManager>().FromComponentInNewPrefab(upgradeManagerPrefab).AsSingle();
    }
}