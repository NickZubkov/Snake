using Leopotam.Ecs;
using TMPro;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class LeaderBoardUI : AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _points;
        
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.LeaderBoardView>().View = this;
        }

        public void ResetLeaderBoard()
        {
            _name.text = "";
            _points.text = "";
        }

        public void SetBoardValue(string name, int points)
        {
            _name.text = name;
            _points.text = $"{points}";
        }
    }
}