using FluentAssertions;
using IssueManager.Core.Models;

namespace IssueManager.Tests.Validators
{
    public class RepoDtoValidatorTests
    {
        private readonly RepoDtoValidator _validator = new();

        [Fact]
        public void Should_Fail_When_Owner_Is_Empty()
        {
            var model = new RepoDto
            {
                Owner = "",
                Repo = "repo",
                Provider = "github",
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Owner");
        }

        [Fact]
        public void Should_Fail_When_Provider_Is_Empty()
        {
            var model = new RepoDto
            {
                Owner = "user",
                Repo = "repo",
                Provider = "",
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Provider");
        }

        [Fact]
        public void Should_Fail_When_Repo_Is_Empty()
        {
            var model = new RepoDto
            {
                Owner = "user",
                Repo = "",
                Provider = "github",
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Repo");
        }

        [Fact]
        public void Should_Pass_When_All_Fields_Are_Valid()
        {
            var model = new RepoDto
            {
                Owner = "user",
                Repo = "repo",
                Provider = "github",
            };

            var result = _validator.Validate(model);

            result.IsValid.Should().BeTrue();
        }
    }
}
