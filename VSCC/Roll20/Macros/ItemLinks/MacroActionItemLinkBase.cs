namespace VSCC.Roll20.Macros.ItemLinks
{
    using System;
    using VSCC.DataType;
    using VSCC.State;

    public abstract class MacroActionItemLinkBase : MacroAction
    {
        public bool TryGetItemLink(Macro m, string name, out InventoryItem ii)
        {
            if (m.ItemsLinked.ContainsKey(name))
            {
                Guid gid = m.ItemsLinked[name];
                foreach (InventoryItem iit in AppState.Current.State.Inventory.Items)
                {
                    if (iit.ObjectID.Equals(gid))
                    {
                        ii = iit;
                        return true;
                    }
                }
            }

            ii = null;
            return false;
        }
    }
}
