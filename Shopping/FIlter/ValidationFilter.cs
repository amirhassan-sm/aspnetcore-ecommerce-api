using Application.Common.BaseModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
                continue;

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argument.GetType());

            var validator = serviceProvider.GetService(validatorType);

            if (validator == null)
                continue;

            var validationResult = await ((IValidator)validator)
                .ValidateAsync(new ValidationContext<object>(argument));

            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(
                    OperationResult.ToFail(
                        "Validation failed",
                        validationResult.Errors
                            .Select(x => x.ErrorMessage)
                            .ToList(),
                        "ValidationFailed"));

                return;
            }
        }

        await next();
    }
}