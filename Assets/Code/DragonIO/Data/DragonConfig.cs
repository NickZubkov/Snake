
namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class DragonConfig
    {
        public Dragons.EntityTemplates.DragonHeadTemplate HeadPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate BodyPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate LegsPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate TailPrefab;
        public float MovementSpeed = 5f;
        public float RotationSpeed = 3f;
        public int Gap = 5;
        public int BodySegmentsCount = 1;
    }
}