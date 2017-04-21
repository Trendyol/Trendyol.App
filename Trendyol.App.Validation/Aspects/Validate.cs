using System;
using System.Reflection;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using PostSharp.Aspects;
using Trendyol.App.Domain.Responses;
using Trendyol.App.Validation.Extensions;

namespace Trendyol.App.Validation.Aspects
{
    [Serializable]
    public class Validate : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            if (args.Arguments == null || args.Arguments.Count == 0)
            {
                throw new InvalidOperationException("A method without and parameters cannot be validated.");
            }

            MethodInfo methodInfo = args.Method as MethodInfo;

            if (methodInfo == null)
            {
                throw new InvalidOperationException("Cannot get method info from PostSharp.");
            }

            if (!methodInfo.ReturnType.IsSubclassOf(typeof(BaseResponse)))
            {
                throw new InvalidOperationException("Only methods that return a type that's inherited from BaseResponse can be validates with this attribute.");
            }

            foreach (object argument in args.Arguments)
            {
                Type requestType = argument.GetType();

                ValidatorAttribute validatorAttribute = requestType.GetCustomAttribute<ValidatorAttribute>();

                if (validatorAttribute != null)
                {
                    IValidator validator = (IValidator)Activator.CreateInstance(validatorAttribute.ValidatorType);
                    ValidationResult validationResult = validator.Validate(args.Arguments[0]);

                    if (!validationResult.IsValid)
                    {
                        BaseResponse response = Activator.CreateInstance(methodInfo.ReturnType) as BaseResponse;

                        if (response == null)
                        {
                            throw new InvalidOperationException($"Cannot initialize a BaseResponse for type {methodInfo.ReturnType.Name}.");
                        }

                        response.Messages = validationResult.GetErrors();

                        args.FlowBehavior = FlowBehavior.Return;
                        args.ReturnValue = response;
                        return;
                    }
                }
            }
        }
    }
}
