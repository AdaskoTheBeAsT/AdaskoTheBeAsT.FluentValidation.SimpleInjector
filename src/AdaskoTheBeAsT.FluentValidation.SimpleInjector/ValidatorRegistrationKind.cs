namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector
{
    public enum ValidatorRegistrationKind
    {
        /// <summary>
        /// Register validator as single open generic.
        /// </summary>
        SingleValidator,

        /// <summary>
        /// Register as collection of open generic validators.
        /// </summary>
        ValidatorCollection,
    }
}
