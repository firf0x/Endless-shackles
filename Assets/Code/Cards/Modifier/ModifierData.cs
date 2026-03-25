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
        
        public ModifierBase Modifier;
        public int Stack;
    }
}