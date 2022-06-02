using UnityEngine;

namespace Modules.DragonIO.LevelController.Components
{
    public struct GoodsSpawningSignal
    {
        public Goods.EntityTemplates.GoodsTemplate GoodsPrefab;
        public Vector3 SpawningPosition;
        public bool UseBodyPosition;
    }
}