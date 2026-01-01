using System;

namespace AquaRegia.Library.Extended.Modules;

[AttributeUsage(AttributeTargets.Property)]
public class RuntimeModuleAttribute : Attribute
{
    public readonly int Order;

    public RuntimeModuleAttribute(int order = 0)
    {
        Order = order;
    }
}