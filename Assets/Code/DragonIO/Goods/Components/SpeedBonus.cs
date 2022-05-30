using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct SpeedBonus : Interfaces.IBonusApplyer
    {
        public float SpeedMultiplier;
        private float _bonusDuration;

        public float BonusDuration
        {
            get => _bonusDuration;
            set => _bonusDuration = value;
        }

        public void Activate(ref DragonHead dragonHead)
        {
            if (dragonHead.SpeedBonusTimer <= 0)
            {
                dragonHead.SpeedBonusMultiplyer = SpeedMultiplier;
                dragonHead.MovementSpeed *= SpeedMultiplier;
            }
            dragonHead.SpeedBonusTimer = BonusDuration;
            dragonHead.SpeedBonusDuration = BonusDuration;
            
        }
    }
}