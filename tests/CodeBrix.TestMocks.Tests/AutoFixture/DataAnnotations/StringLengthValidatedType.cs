using System.ComponentModel.DataAnnotations;

namespace CodeBrix.TestMocks.Tests.AutoFixture.DataAnnotations;  //was previously: namespace AutoFixtureUnitTest.DataAnnotations;

public class StringLengthValidatedType
{
    public const int MaximumLength = 3;

    [StringLength(MaximumLength)]
    public string Property { get; set; }
}