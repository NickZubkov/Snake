using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.DragonIO.Dragons.EntityTemplates
{
    public class DragonHeadTemplate : ViewElement
    {
        [SerializeField] private ParticleSystem _speedVFX;
        [SerializeField] private List<ParticleSystem> _speedPowerUpVFX;
        [SerializeField] private ParticleSystem _shieldVFX;
        [SerializeField] private ParticleSystem _shieldPowerUpVFX;
        [SerializeField] private ParticleSystem _pointVFX;
        [SerializeField] private ParticleSystem _pointPowerUpVFX;
        [SerializeField] private ParticleSystem _winVFX;
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Image _countryImage;
        [SerializeField] private List<MeshRenderer> _viewRenderers;
        
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            ref var dragonHead = ref entity.Get<Components.DragonHead>();
            dragonHead.RotationSpeed = dragonHead.DragonConfig.RotationSpeed;
            dragonHead.MovementSpeed = dragonHead.DragonConfig.MovementSpeed;
            dragonHead.Gap = dragonHead.DragonConfig.Gap;
            dragonHead.DefaultBonusMultiplyer = 1;
            dragonHead.PointBonusMultiplyer = 1;
            dragonHead.HeadTransform = transform;
            dragonHead.BodyParts = new List<Transform>{transform};
            dragonHead.Body = new List<Components.DragonBody>();
            dragonHead.ViewRenderers = _viewRenderers;
            dragonHead.StartBodyCount = dragonHead.DragonConfig.BodySegmentsCount;
            dragonHead.TargetHeadDirection = Vector3.zero;
            dragonHead.Points = 0;
            dragonHead.SpeedVFX = _speedVFX;
            dragonHead.SpeedPowerUpVFX = _speedPowerUpVFX;
            dragonHead.ShieldVFX = _shieldVFX;
            dragonHead.ShieldPowerUpVFX = _shieldPowerUpVFX;
            dragonHead.PointVFX = _pointVFX;
            dragonHead.PointPowerUpVFX = _pointPowerUpVFX;
            dragonHead.WinVFX = _winVFX;
            dragonHead.DragonName = transform.parent.name;
            dragonHead.TextMeshProUGUI = _textMeshPro;
            dragonHead.TextMeshProUGUI.text = dragonHead.DragonName;
            dragonHead.DragonNameColor = _viewRenderers[0].materials[0].color;
            dragonHead.TextMeshProUGUI.color = dragonHead.DragonNameColor;
            dragonHead.CountryImage = _countryImage;
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}