using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class ObstacleVFXProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.Obstacle, Components.ObstaclePlayVFXSignal> _vfxSignal;

        private Data.GameConfig _config;
        public void Run()
        {
            foreach (var vfxSignal in _vfxSignal)
            {
                ref var signal = ref _vfxSignal.Get1(vfxSignal);
                ref var entity = ref _vfxSignal.GetEntity(vfxSignal);
                signal.View.DOScale(Vector3.one * 0.04f, 0.2f);
                signal.PuffVFX.Play();
                entity.Del<Components.Obstacle>();
                entity.Get<Utils.DestroyTag>().DestroyTime = 0.4f;
            }
        }
    }
}