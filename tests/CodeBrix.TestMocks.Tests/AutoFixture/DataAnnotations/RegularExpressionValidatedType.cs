using System.ComponentModel.DataAnnotations;

namespace CodeBrix.TestMocks.Tests.AutoFixture.DataAnnotations;  //was previously: namespace AutoFixtureUnitTest.DataAnnotations;

public class RegularExpressionValidatedType
{
    public const string Pattern = @"^[a-zA-Z''-'\s]{20,40}$";

    [RegularExpression(Pattern)]
    public string Property { get; set; }
}