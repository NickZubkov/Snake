using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class BodyOutLineColorProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<Components.DragonHead>.Exclude<Player.Components.Player> _enemy;
        public void Run()
        {
            foreach (var player in _player)
            {
                ref var playerHead = ref _player.Get1(player);
                foreach (var enemy in _enemy)
                {
                    ref var enemyHead = ref _enemy.Get1(enemy);
                    if (playerHead.BodyParts.Count >= enemyHead.BodyParts.Count)
                    {
                        foreach (var part in enemyHead.Body)
                        {
                            foreach (var renderer in part.ViewRenderers)
                            {
                                var materials = renderer.materials;
                                foreach (var material in materials)
                                {
                                    material.SetColor("_OutlineColor", Color.green);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var part in enemyHead.Body)
                        {
                            foreach (var renderer in part.ViewRenderers)
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
}