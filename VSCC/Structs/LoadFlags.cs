namespace VSCC.Structs
{
    using System;

    [Flags]
    public enum LoadFlags
    {
        None = 0,
        V2AdaptV1 = 1,
        V2NoObjectIDs = 2,
        V2InventoryWeightsMissing = 4
    }
}
