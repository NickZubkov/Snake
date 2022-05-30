using Modules.DragonIO.Dragons.Components;

namespace Modules.DragonIO.Goods.Components
{
    public struct ShieldBonus : Interfaces.IBonusApplyer
    {
        private float _bonusDuration;

        public float BonusDuration
        {
            get => _bonusDuration;
            set => _bonusDuration = value;
        }

        public void Activate(ref DragonHead dragonHead)
        {
            dragonHead.ShieldBonusTimer = BonusDuration;
            dragonHead.ShieldBonusDuration = BonusDuration;
            dragonHead.IsShieldActive = true;
        }
    }
}