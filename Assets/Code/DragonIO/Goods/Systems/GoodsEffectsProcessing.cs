using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsEffectsProcessing : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Components.PlayGoodsEffectTag> _foodSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayBonusVFXSignal> _bonusSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayDeathVFXSignal> _deathSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayWinVFXSignal> _winSignal;

        private Utils.TimeService _timeService;
        public void Run()
        {
            foreach (var signal in _foodSignal)
            {
                ref var view = ref _foodSignal.Get1(signal);
                ref var effect = ref _foodSignal.Get2(signal);
                effect.Timer -= _timeService.DeltaTime;
                
                if (effect.TargetTransform != null && effect.Timer > 0)
                {
                    var direction = effect.TargetTransform.position + (effect.TargetTransform.forward * effect.TargetTransform.localScale.x) - view.Transform.position;
                    direction = direction.Where(y: 0f);
                    view.Transform.Translate(direction * _timeService.DeltaTime * 8f, Space.World);
                    view.Transform.localScale -= Vector3.one * _timeService.DeltaTime * 2f;

                }
                else
                {
                    _foodSignal.GetEntity(signal).Destroy();
                }
                
            }

            foreach (var bonusSignal in _bonusSignal)
            {
                ref var bonusType = ref _bonusSignal.Get2(bonusSignal).BonusType;
                ref var head = ref _bonusSignal.Get1(bonusSignal);

                if (bonusType is Components.PointBonus)
                {
                    head.PointVFX.Play();
                }
                else if (bonusType is Components.ShieldBonus)
                {
                    head.ShieldVFX.Play();
                }
                else if (bonusType is Components.SpeedBonus)
                {
                    head.SpeedVFX.Play();
                }
            }

            foreach (var deathSignal in _deathSignal)
            {
                _deathSignal.Get1(deathSignal).DeathVFX.Play();
            }
            
            foreach (var winSignal in _winSignal)
            {
                _winSignal.Get1(winSignal).WinVFX.Play();
            }
        }
    }
}