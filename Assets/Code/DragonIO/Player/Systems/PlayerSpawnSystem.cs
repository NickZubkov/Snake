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
                    dragonHead.HeadID = -1;
                    playerHeadTemplate.Spawn(dragonHeadEntity, _world);
                    dragonHead.TextMeshProUGUI.transform.parent.gameObject.SetActive(false);
                    dragonHead.DragonNameColor = new Color(0.2245906f, 0.5621458f, 0.9716981f, 1f);
                    ref var body = ref dragonHeadEntity.Get<Dragons.Components.DragonBody>();
                    body.HeadID = dragonHead.HeadID;
                    body.Head = dragonHead;
                    body.ViewRenderers = dragonHead.ViewRenderers;
                    dragonHead.Body.Insert(0, body);
                    
                    _world.NewEntity().Get<Dragons.Components.DragonHeadSpawnedSignal>().DragonHead = dragonHead;
                }
            }
        }
    }
}