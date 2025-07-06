using FluentValidation;

namespace IssueManager.Core.Models.Validators
{
    public class UpdateIssueRequestValidator : AbstractValidator<UpdateIssueRequest>
    {
        public UpdateIssueRequestValidator()
        {
            RuleFor(x => x.Repo).NotNull().SetValidator(new RepoDtoValidator());
            RuleFor(x => x.Title)
                .NotEmpty()
                .When(w =>
                    w.Repo != null
                    && w.Repo.Provider != null
                    && w.Repo.Provider.ToLower() == "gitlab"
                );
        }
    }
}
