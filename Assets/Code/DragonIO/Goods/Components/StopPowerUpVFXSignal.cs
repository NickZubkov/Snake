namespace Modules.DragonIO.Goods.Components
{
    public struct StopPowerUpVFXSignal
    {
        public BonusType BonusType;
    }

    public enum BonusType
    {
        Point,
        Speed,
        Shield
    }
}