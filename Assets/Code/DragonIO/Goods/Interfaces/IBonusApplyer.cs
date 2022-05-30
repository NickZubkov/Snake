namespace Modules.DragonIO.Goods.Interfaces
{
    public interface IBonusApplyer
    {
        float BonusDuration { get; set; }
        void Activate(ref Dragons.Components.DragonHead dragonHead);
    }
}