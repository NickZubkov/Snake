using System.Collections.Generic;
using Leopotam.Ecs;
using Sirenix.OdinInspector;
using TMPro;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UILeaderBoard : AUIEntity
    {
        [Title("First")]
        [SerializeField] private TextMeshProUGUI _nameFirst;
        [SerializeField] private TextMeshProUGUI _pointsFirst;
        [Title("Second")]
        [SerializeField] private TextMeshProUGUI _nameSecond;
        [SerializeField] private TextMeshProUGUI _pointsSecond;
        [Title("Therd")]
        [SerializeField] private TextMeshProUGUI _nameTherd;
        [SerializeField] private TextMeshProUGUI _pointsTherd;
        [Title("Any")]
        [SerializeField] private TextMeshProUGUI _nameAny;
        [SerializeField] private TextMeshProUGUI _pointsAny;
        
        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<Components.LeaderBoard>().View = this;
        }

        public void SetBoardValue(Dragons.Components.DragonHead[] dragonHeads)
        {
            _nameFirst.text = dragonHeads.SafeGetAt(0).DragonName;
            _pointsFirst.text = $"{dragonHeads.SafeGetAt(0).Points}";
            
            _nameSecond.text = dragonHeads.SafeGetAt(1).DragonName;
            _pointsSecond.text = $"{dragonHeads.SafeGetAt(1).Points}";
            
            _nameTherd.text = dragonHeads.SafeGetAt(2).DragonName;
            _pointsTherd.text = $"{dragonHeads.SafeGetAt(2).Points}";
            
            _nameAny.text = dragonHeads.SafeGetAt(3).DragonName;
            _pointsAny.text = $"{dragonHeads.SafeGetAt(3).Points}";
            
        }
    }
}