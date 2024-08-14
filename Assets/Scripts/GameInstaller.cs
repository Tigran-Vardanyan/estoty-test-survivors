using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
   
    public Player player;
    public LootManager lootManager;
    public EnemyManager enemyManager;
    public UIManager uiManager;
    public UpgradeManager upgradeManager;

    public override void InstallBindings()
    {
        Container.Bind<UpgradeManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LootManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<EnemyManager>().FromComponentInHierarchy().AsSingle();


    }
    [Inject]
    public void Construct(EnemyManager enemyManager, UpgradeManager upgradeManager, LootManager lootManager,Player player)
    {
        
        this.enemyManager = enemyManager;
        this.upgradeManager = upgradeManager;
        this.lootManager = lootManager;
        this.player = player;
    }

}