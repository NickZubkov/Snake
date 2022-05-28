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
        public Vector3 TargetHeadDirection;
        public int Points;
        public float MovementSpeed;
        public float RotationSpeed;
        public string DragonName;

        public float DefaultBonusMultiplyer;

        public float SpeedBonusMultiplyer;
        public float SpeedBonusTimer;

        public int PointBonusMultiplyer;
        public float PointBonusTimer;

        public bool IsShieldActive;
        public float ShieldBonusTimer;
    }
}