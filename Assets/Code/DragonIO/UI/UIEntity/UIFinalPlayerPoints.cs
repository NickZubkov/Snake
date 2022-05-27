using Leopotam.Ecs;
using TMPro;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UIFinalPlayerPoints : AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _points;
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.FinalPlayerPoints>().View = this;
        }
        
        public void SetPoints(int points)
        {
            _points.text = $"{points}";
        }
    }
}