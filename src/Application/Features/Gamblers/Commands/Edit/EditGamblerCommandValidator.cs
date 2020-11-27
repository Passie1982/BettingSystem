﻿namespace BettingSystem.Application.Features.Gamblers.Commands.Edit
{
    using FluentValidation;
    using static Domain.Models.ModelConstants.Common;

    public class EditGamblerCommandValidator : AbstractValidator<EditGamblerCommand>
    {
        public EditGamblerCommandValidator()
            => this.RuleFor(g => g.Name)
                .MinimumLength(MinNameLength)
                .MaximumLength(MaxNameLength)
                .NotEmpty();
    }
}