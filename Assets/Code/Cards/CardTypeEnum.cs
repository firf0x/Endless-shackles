using System;

namespace Game.Cards
{
    [Flags]
    public enum CardTypeEnum
    {
        Attack = 1 << 0, // 1
        Defence = 1 << 1, // 2
        Monster = 1 << 2, // 4
    }
}