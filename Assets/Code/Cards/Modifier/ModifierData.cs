using Game.Lib;

namespace Game.Cards.Modifier
{
    public class ModifierData
    {
        public ModifierData(ModifierBase modifier, int stack)
        {
            this.Modifier = modifier;
            this.Stack = stack;
        }
        
        public readonly ModifierBase Modifier;
        public int Stack;
    }
}