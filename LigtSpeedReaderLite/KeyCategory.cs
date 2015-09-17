using System;

namespace LigtSpeedReaderLite
{
    /// <summary>
    /// Represents the list of modifier keys that can been selected for the RegisterHotKey Win32 API.
    /// </summary>
    [Flags]
    public enum KeyCategories
    {
        /// <summary>
        /// No modifier key has been selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// The ALT key modifier has been selected.
        /// </summary>
        AltKey = 1,

        /// <summary>
        /// The CTRL key modifier has been selected.
        /// </summary>
        ControlKey = 2,

        /// <summary>
        /// The SHIFT key, as a modifier, has been selected.
        /// </summary>
        ShiftKey = 4,

        /// <summary>
        /// The WIN key modifier has been selected.
        /// </summary>
        WindowsKey = 8
    }
}