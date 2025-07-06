using FluentValidation;

namespace IssueManager.Core.Models.Validators
{
    public class AddIssueRequestValidator : AbstractValidator<AddIssueRequest>
    {
        public AddIssueRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Repo).NotEmpty();
            RuleFor(x => x.Repo).SetValidator(new RepoDtoValidator());
        }
    }
}
