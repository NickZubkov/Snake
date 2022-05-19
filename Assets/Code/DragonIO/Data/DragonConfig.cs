namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class DragonConfig
    {
        public Dragons.EntityTemplates.DragonHeadTemplate DragonHeadPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabFrontLegs;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabBackLegs;
        public float Speed = 20;
        public int Gap = 10;
        public float BodySize;
    }
}