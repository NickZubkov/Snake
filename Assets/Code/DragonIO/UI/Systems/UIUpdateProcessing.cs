using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;

namespace Modules.DragonIO.UI.Systems
{
    public class UIUpdateProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.LeaderBoard> _leaderBoard;
        private EcsFilter<Components.Timer> _levelTimer;
        private EcsFilter<Components.FinalPlayerPoints> _finalPlayerPoints;
        private EcsFilter<Components.BonusIcons> _bonusIcons;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead> _dragons;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<UICoreECS.UIScreen> _UIScreen;
        
        public void Run()
        {
            foreach (var levelController in _levelController)
            {
                ref var controller = ref _levelController.Get1(levelController);
                
                foreach (var levelTimer in _levelTimer)
                {
                    _levelTimer.Get1(levelTimer).View.SetTimerValue(controller.LevelTimer);
                }
                
                foreach (var playerPoints in _finalPlayerPoints)
                { 
                    _finalPlayerPoints.Get1(playerPoints).View.SetPoints(controller.PlayerPoints);
                }
                
                foreach (var leaderBoard in _leaderBoard)
                {
                    var headList = new List<Dragons.Components.DragonHead>();
                    foreach (var dragon in _dragons)       
                    {
                        headList.Insert(0, _dragons.Get2(dragon));
                    }

                    var sortedList = headList.OrderByDescending(x => x.Points).ToArray();
                
                    _leaderBoard.Get1(leaderBoard).View.SetBoardValue(sortedList);
                }
            }
            foreach (var player in _player)
            {
                ref var playerHead = ref _player.Get1(player);
                
                foreach (var bonusIcon in _bonusIcons)
                {
                    ref var bonus = ref _bonusIcons.Get1(bonusIcon);
                    
                    if (playerHead.PointBonusTimer > 0 && !bonus.PointBonus.activeSelf)
                    {
                        bonus.PointBonus.SetActive(true);
                    }
                    else if (playerHead.PointBonusTimer <= 0 && bonus.PointBonus.activeSelf)
                    {
                        bonus.PointBonus.SetActive(false);
                    }
                    
                    if (playerHead.SpeedBonusTimer > 0 && !bonus.SpeedBonus.activeSelf)
                    {
                        bonus.SpeedBonus.SetActive(true);
                    }
                    else if (playerHead.SpeedBonusTimer <= 0 && bonus.SpeedBonus.activeSelf)
                    {
                        bonus.SpeedBonus.SetActive(false);
                    }
                    
                    if (playerHead.ShieldBonusTimer > 0 && !bonus.ShieldBonus.activeSelf)
                    {
                        bonus.ShieldBonus.SetActive(true);
                    }
                    else if (playerHead.ShieldBonusTimer <= 0 && bonus.ShieldBonus.activeSelf)
                    {
                        bonus.ShieldBonus.SetActive(false);
                    }
                }
            }
        }
    }
}