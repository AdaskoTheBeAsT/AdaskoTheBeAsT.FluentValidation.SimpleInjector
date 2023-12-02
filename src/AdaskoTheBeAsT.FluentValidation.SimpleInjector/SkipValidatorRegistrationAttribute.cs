using System;

namespace AdaskoTheBeAsT.FluentValidation.SimpleInjector;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class SkipValidatorRegistrationAttribute
    : Attribute;
