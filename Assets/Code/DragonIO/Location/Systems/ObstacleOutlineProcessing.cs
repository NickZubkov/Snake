using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class ObstacleOutlineProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.Obstacle> _obstacle;
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _player;
        public void Run()
        {
            foreach (var player in _player)
            {
                ref var playerHead = ref _player.Get1(player);
                foreach (var obstacleComponent in _obstacle)
                {
                    ref var obstacle = ref _obstacle.Get1(obstacleComponent);
                    if (playerHead.BodyParts.Count >= obstacle.DestroyThreshold)
                    {
                        foreach (var renderer in obstacle.ViewMeshRenderers)
                        {
                            var materials = renderer.materials;
                            foreach (var material in materials)
                            {
                                material.SetColor("_OutlineColor", Color.green);
                            }
                        }
                    }
                    else
                    {
                        foreach (var renderer in obstacle.ViewMeshRenderers)
                        {
                            var materials = renderer.materials;
                            foreach (var material in materials)
                            {
                                material.SetColor("_OutlineColor", Color.red);
                            }
                        }
                    }
                }
            }
        }
    }
}