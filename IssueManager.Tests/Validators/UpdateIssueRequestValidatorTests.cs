using FluentAssertions;
using FluentValidation;
using IssueManager.Core.Models;
using IssueManager.Core.Models.Validators;

namespace IssueManager.Tests.Validators
{
    public class UpdateIssueRequestValidatorTests
    {
        private readonly UpdateIssueRequestValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Repo_Is_Invalid()
        {
            var model = new UpdateIssueRequest
            {
                Title = "title",
                Description = "desc",
                Repo = new RepoDto
                {
                    Owner = "", // Invalid
                    Repo = "repo",
                    Provider = "gitlab",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Repo.Owner");
        }

        [Fact]
        public void Should_Fail_When_Repo_Is_Null()
        {
            var model = new UpdateIssueRequest
            {
                Title = "title",
                Description = "desc",
                Repo = null,
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Repo");
        }

        [Fact]
        public void Should_Fail_When_Title_Is_Empty_And_Provider_Is_GitLab()
        {
            var model = new UpdateIssueRequest
            {
                Title = "",
                Description = "Some desc",
                Repo = new RepoDto
                {
                    Owner = "test",
                    Repo = "repo",
                    Provider = "gitlab",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title");
        }

        [Fact]
        public void Should_Pass_When_Title_Is_Empty_And_Provider_Is_GitHub()
        {
            var model = new UpdateIssueRequest
            {
                Title = "",
                Description = "desc",
                Repo = new RepoDto
                {
                    Owner = "owner",
                    Repo = "repo",
                    Provider = "github",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Pass_When_Title_Is_Set_For_GitLab()
        {
            var model = new UpdateIssueRequest
            {
                Title = "Valid title",
                Description = "desc",
                Repo = new RepoDto
                {
                    Owner = "owner",
                    Repo = "repo",
                    Provider = "gitlab",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeTrue();
        }
    }
}
