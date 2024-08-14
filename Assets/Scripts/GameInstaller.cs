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
        Container.Bind<UpgradeManager>().FromInstance(upgradeManager);
        Container.Bind<Player>().FromInstance(player);
        Container.Bind<LootManager>().FromInstance(lootManager);
        Container.Bind<EnemyManager>().FromInstance(enemyManager);
        Container.Bind<UIManager>().FromInstance(uiManager);
        Container.Inject(upgradeManager);
        Container.Inject(player);
        Container.Inject(lootManager);
        Container.Inject(enemyManager);
        Container.Inject(uiManager);


    }
    
}