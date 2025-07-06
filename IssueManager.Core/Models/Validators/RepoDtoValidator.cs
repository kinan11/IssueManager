using FluentValidation;
using IssueManager.Core.Models;

public class RepoDtoValidator : AbstractValidator<RepoDto>
{
    public RepoDtoValidator()
    {
        RuleFor(x => x.Owner).NotEmpty();
        RuleFor(x => x.Provider).NotEmpty();
        RuleFor(x => x.Repo).NotEmpty();
    }
}

