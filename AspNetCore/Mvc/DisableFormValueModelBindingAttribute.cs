using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Finaps.Commons.AspNetCore.Mvc
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
  {
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
      var factories = context.ValueProviderFactories;
      factories.RemoveType<FormValueProviderFactory>();
      factories.RemoveType<JQueryFormValueProviderFactory>();
    }
  }
}
