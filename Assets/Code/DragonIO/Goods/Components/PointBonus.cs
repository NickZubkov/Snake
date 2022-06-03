using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct PointBonus : Interfaces.IBonusApplyer
    {
        public int PointMultiplyer;
        private float _bonusDuration;
        public float BonusDuration
        {
            get => _bonusDuration;
            set => _bonusDuration = value;
        }

        public void Activate(ref DragonHead dragonHead)
        {
            if (dragonHead.PointBonusTimer <= 0)
            {
                dragonHead.PointBonusMultiplyer = PointMultiplyer;
            }
            dragonHead.PointBonusTimer = BonusDuration;
            dragonHead.PointBonusDuration = BonusDuration;
        }
    }
}