using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.AutoFixture.AutoMock; //was previously: namespace AutoFixture.AutoMoq;

/// <summary>
/// Enables auto-mocking and auto-setup with Mock.
/// Members of a mock will be automatically setup to retrieve the return values from a fixture.
/// </summary>
[Obsolete("This customization is obsolete. " +
          "Please use 'new AutoMockCustomization { ConfigureMembers = true }' customization instead.")]
public class AutoConfiguredMockCustomization : AutoMockCustomization
{
    /// <summary>
    /// Creates a new instance of <see cref="AutoConfiguredMockCustomization"/>.
    /// </summary>
    public AutoConfiguredMockCustomization()
    {
        this.ConfigureMembers = true;
    }

    /// <summary>
    /// Creates a new instance of <see cref="AutoConfiguredMockCustomization"/>.
    /// </summary>
    public AutoConfiguredMockCustomization(ISpecimenBuilder relay)
        : base(relay)
    {
        this.ConfigureMembers = true;
    }
}