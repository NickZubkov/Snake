using Leopotam.Ecs;
using TMPro;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UITimer : AUIEntity
    {
        [SerializeField] private TextMeshProUGUI _timer;
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.Timer>().View = this;
        }
        public void SetTimerValue(float time)
        {
            _timer.text = $"{time:F1}";
        }
    }
}