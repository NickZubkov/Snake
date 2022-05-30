using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Goods.ViewComponents
{
    public class BonusViewComponent : ViewComponent
    {
        [SerializeField] private BonusComponent _bonusComponent;
        [SerializeField] private float _bonusDuration;
        [SerializeField] private float _bonusMultiplyer;
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ref var entityBonus = ref ecsEntity.Get<Components.Bonus>();
            
            switch (_bonusComponent)
            {
                case BonusComponent.PointBonus :
                    entityBonus.BonusApplyer = new Components.PointBonus
                    {
                        BonusDuration = _bonusDuration,
                        PointMultiplyer = (int)_bonusMultiplyer
                    };
                    break;
                case BonusComponent.ShieldBonus :
                    entityBonus.BonusApplyer = new Components.ShieldBonus
                    {
                        BonusDuration = _bonusDuration
                    };
                    break;
                case BonusComponent.SpeedBonus :
                    entityBonus.BonusApplyer = new Components.SpeedBonus
                    {
                        BonusDuration = _bonusDuration,
                        SpeedMultiplier = _bonusMultiplyer
                    };
                    break;
                default:
                    entityBonus.BonusApplyer = new Components.SpeedBonus
                    {
                        BonusDuration = _bonusDuration,
                        SpeedMultiplier = _bonusMultiplyer
                    };
                    break;
            }
            
            ecsEntity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
    
    public enum BonusComponent
    {
        PointBonus,
        SpeedBonus,
        ShieldBonus
    }
}