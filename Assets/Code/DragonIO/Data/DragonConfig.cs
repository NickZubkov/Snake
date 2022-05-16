using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class DragonConfig : ScriptableObject
    {
        public Dragons.EntityTemplates.DragonHeadTemplate DragonHeadPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabFrontLegs;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabBackLegs;
        public float Speed = 5;
        public int Gap = 10;
    }
}