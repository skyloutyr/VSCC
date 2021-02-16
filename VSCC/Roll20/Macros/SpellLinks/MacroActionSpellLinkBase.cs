namespace VSCC.Roll20.Macros.SpellLinks
{
    using System;
    using VSCC.DataType;
    using VSCC.State;

    public abstract class MacroActionSpellLinkBase : MacroAction
    {
        public bool TryGetSpellLink(Macro m, string name, out Spell s)
        {
            if (m.SpellsLinked.ContainsKey(name))
            {
                Guid gid = m.SpellsLinked[name];
                foreach (Spell sp in AppState.Current.State.Spellbook.AllSpells)
                {
                    if (sp.ObjectID.Equals(gid))
                    {
                        s = sp;
                        return true;
                    }
                }
            }

            s = null;
            return false;
        }
    }
}
