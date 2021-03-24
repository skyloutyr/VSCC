namespace VSCC.Structs
{
    using System;

    [Flags]
    public enum LoadFlags
    {
        None = 0,
        V2AdaptV1 = 1,
        V2NoObjectIDs = 2,
        V2InventoryWeightsMissing = 4,
        V2OldFeats = 8,
        V2OldImageModels = 16,
        KeepLoadObject = 0b01000000000000000000000000000000
    }
}
