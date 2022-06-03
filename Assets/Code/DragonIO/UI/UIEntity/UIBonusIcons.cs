using Leopotam.Ecs;
using UICoreECS;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.DragonIO.UI.UIEntity
{
    public class UIBonusIcons : AUIEntity
    {
        [SerializeField] private GameObject _speedBonus;
        [SerializeField] private GameObject _pointBonus;
        [SerializeField] private GameObject _shieldBonus;
        [SerializeField] private Image _speedBonusImage;
        [SerializeField] private Image _pointBonusImage;
        [SerializeField] private Image _shieldBonusImage;


        public override void Init(EcsWorld world, EcsEntity screen)
        {
            ref var entity = ref screen.Get<Components.BonusIcons>();

            entity.PointBonus = _pointBonus;
            entity.ShieldBonus = _shieldBonus;
            entity.SpeedBonus = _speedBonus;
            entity.PointBonusImage = _pointBonusImage;
            entity.ShieldBonusImage = _shieldBonusImage;
            entity.SpeedBonusImage = _speedBonusImage;
            _pointBonus.SetActive(false);
            _shieldBonus.SetActive(false);
            _speedBonus.SetActive(false);
        }
    }
}