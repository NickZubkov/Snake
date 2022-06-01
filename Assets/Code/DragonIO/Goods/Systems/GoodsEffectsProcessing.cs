using Leopotam.Ecs;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsEffectsProcessing : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Components.PlayGoodsEffectTag> _foodSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayBonusVFXSignal> _playVFXSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayDeathVFXSignal> _deathSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayWinVFXSignal> _winSignal;
        private EcsFilter<Dragons.Components.DragonHead, Components.StopPowerUpVFXSignal> _stopPowerupVFXSignal;

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
                    if (effect.IsFood)
                    {
                        Misc.PlayVibro(HapticTypes.MediumImpact);
                    }
                    else
                    {
                        Misc.PlayVibro(HapticTypes.HeavyImpact);
                    }
                    
                    _foodSignal.GetEntity(signal).Destroy();
                }
                
            }

            foreach (var bonusSignal in _playVFXSignal)
            {
                ref var bonusType = ref _playVFXSignal.Get2(bonusSignal).BonusType;
                ref var head = ref _playVFXSignal.Get1(bonusSignal);

                if (bonusType is Components.PointBonus)
                {
                    head.PointVFX.Play();
                    head.PointPowerUpVFX.Play();
                }
                else if (bonusType is Components.ShieldBonus)
                {
                    head.ShieldVFX.Play();
                    head.ShieldPowerUpVFX.Play();
                }
                else if (bonusType is Components.SpeedBonus)
                {
                    head.SpeedVFX.Play();
                    foreach (var vfx in head.SpeedPowerUpVFX)
                    {
                        vfx.Play();
                    }
                }
            }

            foreach (var stopSignal in _stopPowerupVFXSignal)
            {
                ref var bonusType = ref _stopPowerupVFXSignal.Get2(stopSignal).BonusType;
                ref var head = ref _stopPowerupVFXSignal.Get1(stopSignal);
                switch (bonusType)
                {
                    case Components.BonusType.Point :
                        head.PointPowerUpVFX.Stop();
                        break;
                    case Components.BonusType.Shield :
                        head.ShieldPowerUpVFX.Stop();
                        break;
                    case Components.BonusType.Speed :
                        foreach (var vfx in head.SpeedPowerUpVFX)
                        {
                            vfx.Stop();
                        }
                        break;
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