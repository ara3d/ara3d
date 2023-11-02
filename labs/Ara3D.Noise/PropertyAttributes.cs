using System;

namespace Unity.Mathematics
{
    /// <summary>
    /// Used by property drawers when vectors should be post normalized.
    /// </summary>
    public class PostNormalizeAttribute : Attribute {}

    /// <summary>
    /// Used by property drawers when vectors should not be normalized.
    /// </summary>
    public class DoNotNormalizeAttribute : Attribute {}
}
