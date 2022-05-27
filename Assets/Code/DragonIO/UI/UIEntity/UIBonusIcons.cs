using Leopotam.Ecs;
using UICoreECS;
using UnityEngine;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UIBonusIcons : AUIEntity
    {
        [SerializeField] private GameObject _speedBonus;
        [SerializeField] private GameObject _pointBonus;
        [SerializeField] private GameObject _shieldBonus;


        public override void Init(EcsWorld world, EcsEntity screen)
        {
            ref var entity = ref screen.Get<Components.BonusIcons>();

            entity.PointBonus = _pointBonus;
            entity.ShieldBonus = _shieldBonus;
            entity.SpeedBonus = _speedBonus;
            _pointBonus.SetActive(false);
            _shieldBonus.SetActive(false);
            _speedBonus.SetActive(false);
        }
    }
}