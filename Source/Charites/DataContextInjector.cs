// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to inject a data context to the controller.
/// </summary>
public class DataContextInjector : IDataContextInjector
{
    /// <summary>
    /// Gets the <see cref="BindingFlags"/> for a data context.
    /// </summary>
    protected virtual BindingFlags DataContextBindingFlags => BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

    /// <summary>
    /// Injects the specified data context to fields of the specified controller.
    /// </summary>
    /// <param name="dataContext">The data context that is injected to fields of the controller.</param>
    /// <param name="controller">The controller to inject a data context.</param>
    protected virtual void InjectDataContextToField(object? dataContext, object controller)
        => controller.GetType()
            .GetFields(DataContextBindingFlags)
            .Where(field => field.GetCustomAttribute<DataContextAttribute>(true) is not null)
            .ForEach(field => InjectDataContext(controller, () => field.SetValue(controller, dataContext)));

    /// <summary>
    /// Injects the specified data context to properties of the specified controller.
    /// </summary>
    /// <param name="dataContext">The data context that is injected to the controller.</param>
    /// <param name="controller">The controller to inject a data context.</param>
    protected virtual void InjectDataContextToProperty(object? dataContext, object controller)
        => controller.GetType()
            .GetProperties(DataContextBindingFlags)
            .Where(property => property.GetCustomAttribute<DataContextAttribute>(true) is not null)
            .ForEach(property => InjectDataContext(controller, () => property.SetValue(controller, dataContext)));

    /// <summary>
    /// Injects the specified data context to methods of the specified controller.
    /// </summary>
    /// <param name="dataContext">The data context that is injected to the controller.</param>
    /// <param name="controller">The controller to inject a data context.</param>
    protected virtual void InjectDataContextToMethod(object? dataContext, object controller)
        => controller.GetType()
            .GetMethods(DataContextBindingFlags)
            .Where(method => method.GetCustomAttribute<DataContextAttribute>(true) is not null)
            .ForEach(method => InjectDataContext(controller, () => method.Invoke(controller, new[] { dataContext })));

    /// <summary>
    /// Injects the specified data context to methods of the specified controller using a naming convention.
    /// </summary>
    /// <param name="dataContext">The data context that is injected to the controller.</param>
    /// <param name="controller">The controller to inject a data context.</param>
    protected virtual void InjectDataContextToMethodUsingNamingConvention(object? dataContext, object controller)
        => controller.GetType()
            .GetMethods(DataContextBindingFlags)
            .Where(method => method.Name == "SetDataContext")
            .Where(method => method.GetCustomAttribute<DataContextAttribute>(true) is null)
            .ForEach(method => InjectDataContext(controller, () => method.Invoke(controller, new[] { dataContext })));

    /// <summary>
    /// Injects the specified data context using the specified action.
    /// </summary>
    /// <param name="controller">The controller to inject a data context.</param>
    /// <param name="action">The action that injects the data context.</param>
    protected void InjectDataContext(object controller, Action action)
    {
        try
        {
            action();
        }
        catch (Exception exc)
        {
            throw new DataContextInjectionException($"The injection of a data context to {controller.GetType()} is failed", exc);
        }
    }

    void IDataContextInjector.Inject(object? dataContext, object controller)
    {
        InjectDataContextToField(dataContext, controller);
        InjectDataContextToProperty(dataContext, controller);
        InjectDataContextToMethod(dataContext, controller);
        InjectDataContextToMethodUsingNamingConvention(dataContext, controller);
    }
}