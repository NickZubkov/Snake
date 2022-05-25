namespace Modules.DragonIO.Goods.Interfaces
{
    public interface IBonusApplyer
    {
        float BonusTimeLife { get; set; }
        void Activate(ref Dragons.Components.DragonHead dragonHead);
    }
}