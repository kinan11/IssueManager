using FluentAssertions;
using IssueManager.Core.Models;
using IssueManager.Core.Models.Validators;

namespace IssueManager.Tests.Validators
{
    public class AddIssueRequestValidatorTests
    {
        private readonly AddIssueRequestValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Title_Is_Empty()
        {
            var model = new AddIssueRequest
            {
                Title = "",
                Description = "Some description",
                Repo = new RepoDto
                {
                    Owner = "test",
                    Repo = "repo",
                    Provider = "github",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title");
        }

        [Fact]
        public void Should_Fail_When_Description_Is_Empty()
        {
            var model = new AddIssueRequest
            {
                Title = "Test title",
                Description = "",
                Repo = new RepoDto
                {
                    Owner = "test",
                    Repo = "repo",
                    Provider = "github",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Description");
        }

        [Fact]
        public void Should_Fail_When_Repo_Is_Null()
        {
            var model = new AddIssueRequest
            {
                Title = "Valid title",
                Description = "Valid description",
                Repo = null,
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Repo");
        }

        [Fact]
        public void Should_Fail_When_Repo_Is_Invalid()
        {
            var model = new AddIssueRequest
            {
                Title = "Valid title",
                Description = "Valid description",
                Repo = new RepoDto
                {
                    Owner = "", // Invalid
                    Repo = "", // Invalid
                    Provider = "", // Invalid
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Repo"));
        }

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var model = new AddIssueRequest
            {
                Title = "Valid title",
                Description = "Valid description",
                Repo = new RepoDto
                {
                    Owner = "test",
                    Repo = "repo",
                    Provider = "github",
                },
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeTrue();
        }
    }
}
