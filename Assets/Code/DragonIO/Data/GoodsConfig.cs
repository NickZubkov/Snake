using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GoodsConfig
    {
        public Goods.EntityTemplates.GoodsTemplate FoodPrefab;
        public Goods.EntityTemplates.GoodsTemplate BonusPrefab;
        [Range(1, 500)]
        public int MinFoodCount = 20;
        [Range(1, 100)] 
        public int MaxBonusCount = 10;
        [MinMaxSlider(1, 10)]
        public Vector2Int BonusSpawnTimeRange = new (1,2);
    }
}