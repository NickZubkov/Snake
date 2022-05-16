using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GoodsConfig", menuName = "Modules/DragonIO/GoodsConfig")]
    public class GoodsConfig : ScriptableObject
    {
        public Goods.EntityTemplates.GoodsTemplate FoodPrefab;
        public Goods.EntityTemplates.GoodsTemplate BonusPrefab;
        [Range(10, 500)]
        public int MinFoodCount = 20;
        [Range(1, 100)] 
        public int MaxBonusCount = 10;

        public BonusSpawnTimeRange BonusSpawnTimeRange;
    }

    [System.Serializable]
    public class BonusSpawnTimeRange
    {
        public int Min = 5;
        public int Max = 15;
    }
}