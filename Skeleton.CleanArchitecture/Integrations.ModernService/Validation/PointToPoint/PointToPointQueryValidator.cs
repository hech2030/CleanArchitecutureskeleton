using FluentValidation;
using Integrations.ModernService.Application.Common.Queries;
using Integrations.ModernService.Domain.Entities.Common.Validation;

namespace Integrations.ModernService.Validation.PointToPoint
{
    public sealed class MyEndpointQueryValidator : AbstractValidator<MyEndpointQuery>
    {
        private const int MaxCursorLength = 1024;

        public MyEndpointQueryValidator()
        {

            RuleLevelCascadeMode = CascadeMode.Stop;

            When(
                x => !string.IsNullOrWhiteSpace(x.Cursor),
                () =>
                {

                    RuleFor(x => x.Cursor)
                    .Must(x => x!.Length < MaxCursorLength)
                    .WithMessage("OUT_OF_RANGE_VALUE")
                    .WithState(_ => new ValidatorState(ValidationErrorCodes.INVALID_DATA));
                });
        }
    }
}
