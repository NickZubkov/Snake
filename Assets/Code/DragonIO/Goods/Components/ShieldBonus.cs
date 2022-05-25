using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct ShieldBonus : Interfaces.IBonusApplyer
    {
        private float _bonusTimeLife;

        public float BonusTimeLife
        {
            get => _bonusTimeLife;
            set => _bonusTimeLife = value;
        }

        public void Activate(ref DragonHead dragonHead)
        {
            dragonHead.ShieldBonusTimer += BonusTimeLife;
            dragonHead.IsShieldActive = true;
        }
    }
}