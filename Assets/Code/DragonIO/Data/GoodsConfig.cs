using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GoodsConfig
    {
        public Goods.EntityTemplates.GoodsTemplate FoodPrefab;
        public List<Goods.EntityTemplates.GoodsTemplate> BonusPrefabs = new List<Goods.EntityTemplates.GoodsTemplate>
        {
            new Goods.EntityTemplates.GoodsTemplate()
        };
        [Range(1, 500)]
        public int MinFoodCount = 20;
        [Range(1, 100)] 
        public int MaxBonusCount = 10;
        [MinMaxSlider(1, 10)]
        public Vector2Int BonusSpawnTimeRange = new Vector2Int(1,2);
    }
}