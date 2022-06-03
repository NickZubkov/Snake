using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.UI.Systems
{
    public class UIUpdateProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.LeaderBoard> _leaderBoard;
        private EcsFilter<Components.Timer> _levelTimer;
        private EcsFilter<Components.FinalPlayerPoints> _finalPlayerPoints;
        private EcsFilter<Components.BonusIcons> _bonus;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead> _dragons;
        private EcsFilter<LevelController.Components.LevelRunTimeData> _levelController;
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<Components.FlyingTextSignal> _flyingSignal;
        private EcsFilter<Components.FlyingText> _flyingText;
        
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
                
                foreach (var bonus in _bonus)
                {
                    ref var bonusIcon = ref _bonus.Get1(bonus);
                    
                    if (playerHead.PointBonusTimer > 0)
                    {
                        if (!bonusIcon.PointBonus.activeSelf)
                            bonusIcon.PointBonus.SetActive(true);
                        
                        bonusIcon.PointBonusImage.fillAmount = playerHead.PointBonusTimer / playerHead.PointBonusDuration;
                    }
                    else if (playerHead.PointBonusTimer <= 0 && bonusIcon.PointBonus.activeSelf)
                    {
                        bonusIcon.PointBonus.SetActive(false);
                    }
                    
                    if (playerHead.SpeedBonusTimer > 0 )
                    {
                        if (!bonusIcon.SpeedBonus.activeSelf)
                            bonusIcon.SpeedBonus.SetActive(true);
                        
                        bonusIcon.SpeedBonusImage.fillAmount = playerHead.SpeedBonusTimer / playerHead.SpeedBonusDuration;
                    }
                    else if (playerHead.SpeedBonusTimer <= 0 && bonusIcon.SpeedBonus.activeSelf)
                    {
                        bonusIcon.SpeedBonus.SetActive(false);
                    }
                    
                    if (playerHead.ShieldBonusTimer > 0)
                    {
                        if (!bonusIcon.ShieldBonus.activeSelf)
                            bonusIcon.ShieldBonus.SetActive(true);
                        
                        bonusIcon.ShieldBonusImage.fillAmount = playerHead.ShieldBonusTimer / playerHead.ShieldBonusDuration;
                    }
                    else if (playerHead.ShieldBonusTimer <= 0 && bonusIcon.ShieldBonus.activeSelf)
                    {
                        bonusIcon.ShieldBonus.SetActive(false);
                    }
                }
            }

            foreach (var flyingSignal in _flyingSignal)
            {
                foreach (var flyingText in _flyingText)
                {
                    _flyingText.Get1(flyingText).View.RunText();
                }

                _flyingSignal.GetEntity(flyingSignal).Get<Utils.DestroyTag>();
            }
        }
    }
}