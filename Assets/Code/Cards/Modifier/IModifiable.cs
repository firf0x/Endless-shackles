using System.Collections.Generic;

namespace Game.Lib
{
    public interface IModifiable<ModBase> 
    {
        List<ModBase> Modifiers { get; }
        void AddModifier(ModBase modifier);
        void RemoveModifier(ModBase modifier);
        bool HasModifier(ModBase modifier);
        List<T> GetModifiersByType<T>() where T : ModBase;
    }
}