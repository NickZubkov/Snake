using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Components
{
    public struct DragonHead
    {
        public Data.DragonConfig DragonConfig;
        public int HeadID;
        public Transform HeadTransform;
        public List<Transform> BodyParts;
        public int StartBodyCount;
        public Vector3 TargetHeadDirection;
        public bool LockDirection;
        public float LockDirectionTimer;
        public int Points;
        public float MovementSpeed;
        public float RotationSpeed;
        public float Gap;
        public string DragonName;
        
        public ParticleSystem SpeedVFX;
        public List<ParticleSystem> SpeedPowerUpVFX;
        public ParticleSystem ShieldVFX;
        public ParticleSystem ShieldPowerUpVFX;
        public ParticleSystem PointVFX;
        public ParticleSystem PointPowerUpVFX;
        public ParticleSystem DeathVFX;
        public ParticleSystem WinVFX;

        public float DefaultBonusMultiplyer;

        public float SpeedBonusMultiplyer;
        public float SpeedBonusDuration;
        public float SpeedBonusTimer;

        public int PointBonusMultiplyer;
        public float PointBonusDuration;
        public float PointBonusTimer;

        public bool IsShieldActive;
        public float ShieldBonusDuration;
        public float ShieldBonusTimer;
    }
}