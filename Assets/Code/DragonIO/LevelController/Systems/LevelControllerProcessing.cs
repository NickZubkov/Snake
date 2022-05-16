using Leopotam.Ecs;
using Modules.Utils;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        private EcsFilter<Goods.Components.Food> _food;
        private EcsFilter<Goods.Components.Bonus> _bonus;
        private TimeService _timeService;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _controller)
            {
                ref var controller = ref _controller.Get1(idx);

                controller.BonusSpawnTimer -= _timeService.DeltaTime;
                
                if (_food.GetEntitiesCount() < controller.MinFoodCount)
                {
                    _controller.GetEntity(idx).Get<Components.FoodSpawningSignal>();
                }
                
                if (controller.BonusSpawnTimer <= 0)
                {
                    if (_bonus.GetEntitiesCount() < controller.MaxBonusCount)
                    {
                        _controller.GetEntity(idx).Get<Components.BonusSpawningSignal>();
                    }
                    
                    controller.BonusSpawnTimer = Random.Range(controller.BonusMinSpawnTime, controller.BonusMaxSpawnTime);
                }
            }
        }
    }
}