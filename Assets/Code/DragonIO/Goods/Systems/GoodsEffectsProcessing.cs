using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsEffectsProcessing : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Components.PlayGoodsEffectTag> _signal;

        private Utils.TimeService _timeService;
        public void Run()
        {
            foreach (var signal in _signal)
            {
                ref var view = ref _signal.Get1(signal);
                ref var effect = ref _signal.Get2(signal);
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
                    _signal.GetEntity(signal).Destroy();
                }
                
            }
        }
    }
}