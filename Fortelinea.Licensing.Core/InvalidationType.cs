namespace Fortelinea.Licensing.Core
{
    /// <summary>
    ///     InvalidationType
    /// </summary>
    public enum InvalidationType
    {
        /// <summary>
        ///     Can not create a new license
        /// </summary>
        CannotGetNewLicense,

        /// <summary>
        ///     License is expired
        /// </summary>
        TimeExpired
    }
}