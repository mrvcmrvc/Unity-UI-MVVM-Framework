using MVVM;
using System;
using System.Collections.Generic;
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

public class BindablePropertyNotFoundException : Exception
{
    private const string message = "Property \"{0}\" is not found in \"{1}\"";
    public BindablePropertyNotFoundException(VMBase targetVM, Type pldType)
        : base(string.Format(message, pldType.ToString(), targetVM.name))
    {
    }
}

public class BindableMethodNotFoundException : Exception
{
    private const string message = "Method \"{0}\" is not found in \"{1}\"";
    public BindableMethodNotFoundException(VMBase targetVM, string methodName)
        : base(string.Format(message, methodName, targetVM.name))
    {
    }
}

public static class BindingExtensions
{
    public static PropertyInfo GetPropertyInfoOf<TPLD>(VMBase viewModel)
        where TPLD : IPLDBase
    {
        PropertyInfo[] propInfoColl = viewModel.GetType()
            .GetRuntimeProperties()
            .Where(m => m.GetCustomAttributes(typeof(ViewModelToViewAttribute), false).Length > 0)
            .Where(m => m.PropertyType.Equals(typeof(TPLD)))
            .ToArray();

        if (propInfoColl.Length == 0)
            throw new BindablePropertyNotFoundException(viewModel, typeof(TPLD));

        return propInfoColl[0];
    }

    public static PropertyInfo GetListPropertyInfoOf<TPLD>(VMBase viewModel)
    where TPLD : IPLDBase
    {
        PropertyInfo[] propInfoColl = viewModel.GetType()
            .GetRuntimeProperties()
            .Where(m => m.GetCustomAttributes(typeof(ViewModelToViewAttribute), false).Length > 0)
            .Where(m => m.PropertyType.Equals(typeof(List<TPLD>)))
            .ToArray();

        if (propInfoColl.Length == 0)
            throw new BindablePropertyNotFoundException(viewModel, typeof(List<TPLD>));

        return propInfoColl[0];
    }

    public static MethodInfo GetMethodInfoOf(VMBase viewModel, string targetMethodName)
    {
        MethodInfo[] methodInfoColl = viewModel.GetType()
            .GetRuntimeMethods()
            .Where(m => m.GetCustomAttributes(typeof(ViewToViewModelAttribute), false).Length > 0)
            .Where(m => ((ViewToViewModelAttribute)m.GetCustomAttribute(typeof(ViewToViewModelAttribute))).UniqueName.Equals(targetMethodName))
            .ToArray();

        if (methodInfoColl.Length == 0)
            throw new BindableMethodNotFoundException(viewModel, targetMethodName);

        return methodInfoColl[0];
    }
}