using MVVM;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ViewModelToViewAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method)]
public class ViewToViewModelAttribute : Attribute
{
    public string UniqueName { get; private set; }

    public ViewToViewModelAttribute([CallerMemberName] string methodName = null)
    {
        UniqueName = methodName;
    }
}

public static class BindingExtensions
{
    public static TPLD GetPLDValue<TPLD>(VMBase viewModel) where TPLD : IPLDBase
    {
        PropertyInfo[] propInfoColl = viewModel.GetType()
            .GetRuntimeProperties()
            .Where(m => m.GetCustomAttributes(typeof(ViewModelToViewAttribute), false).Length > 0)
            .Where(m => m.PropertyType.Equals(typeof(TPLD)))
            .ToArray();

        return (TPLD)propInfoColl[0].GetValue(viewModel);
    }

    public static PropertyInfo GetPropertyInfoOf<TPLD>(VMBase viewModel)
        where TPLD : IPLDBase
    {
        PropertyInfo[] propInfoColl = viewModel.GetType()
            .GetRuntimeProperties()
            .Where(m => m.GetCustomAttributes(typeof(ViewModelToViewAttribute), false).Length > 0)
            .Where(m => m.PropertyType.Equals(typeof(TPLD)))
            .ToArray();

        return propInfoColl[0];
    }

    public static MethodInfo GetMethodInfoOf(VMBase viewModel, string targetMethodName)
    {
        MethodInfo[] methodInfoColl = viewModel.GetType()
            .GetRuntimeMethods()
            .Where(m => m.GetCustomAttributes(typeof(ViewToViewModelAttribute), false).Length > 0)
            .Where(m => ((ViewToViewModelAttribute)m.GetCustomAttribute(typeof(ViewToViewModelAttribute))).UniqueName.Equals(targetMethodName))
            .ToArray();

        return methodInfoColl[0];
    }
}