using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct SpeedBonus : Interfaces.IBonusApplyer
    {
        public float SpeedMultiplier;
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
                dragonHead.SpeedBonusMultiplyer = SpeedMultiplier;
                dragonHead.MovementSpeed *= SpeedMultiplier;
            }
            dragonHead.SpeedBonusTimer += BonusTimeLife;
            
        }
    }
}