using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct PointBonus : Interfaces.IBonusApplyer
    {
        public int PointMultiplyer;
        private float _bonusTimeLife;
        public float BonusTimeLife
        {
            get => _bonusTimeLife;
            set => _bonusTimeLife = value;
        }

        public void Activate(ref DragonHead dragonHead)
        {
            if (dragonHead.SpeedBonusTimer <= 0)
            {
                dragonHead.PointBonusMultiplyer = PointMultiplyer;
            }
            dragonHead.PointBonusTimer += BonusTimeLife;
        }
    }
}