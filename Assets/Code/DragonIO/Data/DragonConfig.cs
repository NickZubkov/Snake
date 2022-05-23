using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class DragonConfig
    {
        public Dragons.EntityTemplates.DragonHeadTemplate HeadPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate BodyPrefab;
        [Tooltip(nameof(BodyPrefabFrontLegs))]
        public Dragons.EntityTemplates.DragonBodyTemplate BodyPrefabFrontLegs;
        [Tooltip(nameof(BodyPrefabBackLegs))]
        public Dragons.EntityTemplates.DragonBodyTemplate BodyPrefabBackLegs;
        public float MovementSpeed = 5f;
        public float RotationSpeed = 5f;
        public int Gap = 6;
        public float BodySize = 1;
        public int BodySegmentsCount = 1;
    }
}