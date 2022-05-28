using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.PlayerSpawningSignal> _playerSpawningSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        
        private EcsWorld _world;

        public void Run()
        {
            if (!_playerSpawningSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    var parent = new GameObject("Player");
                    var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                    parentEntity._components = new List<ViewHub.ViewComponent>();
                    parentEntity.Spawn(_world.NewEntity(), _world);
                    
                    var playerHeadTemplate = Object.Instantiate(currentLevelConfigs.PlayerConfig.HeadPrefab, Vector3.zero, Quaternion.identity);
                    playerHeadTemplate.transform.parent = parent.transform;
                    
                    var dragonHeadEntity = _world.NewEntity();
                    dragonHeadEntity.Get<Player.Components.Player>();
                    ref var dragonHead = ref dragonHeadEntity.Get<Dragons.Components.DragonHead>();
                    dragonHead.DragonConfig = currentLevelConfigs.PlayerConfig;
                    playerHeadTemplate.Spawn(dragonHeadEntity, _world);
                    
                    _world.NewEntity().Get<Dragons.Components.DragonHeadSpawnedSignal>().DragonHead = dragonHead;
                }
            }
        }
    }
}