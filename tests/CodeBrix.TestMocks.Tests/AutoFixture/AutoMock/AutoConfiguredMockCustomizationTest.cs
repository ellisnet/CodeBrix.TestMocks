using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.AutoMock;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using CodeBrix.TestMocks.Mocking;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock; //was previously: namespace AutoFixture.AutoMoq.UnitTest;

[Obsolete]
public class AutoConfiguredMockCustomizationTest
{
    [Fact]
    public void IsInheritedFromAutoMockCustomization()
    {
        // Arrange
        var sut = new AutoConfiguredMockCustomization();
        // Act & Assert
        Assert.IsAssignableFrom<AutoMockCustomization>(sut);
    }

    [Fact]
    public void SetsConfigureMembersPropertyToTrue()
    {
        // Arrange
        var sut = new AutoConfiguredMockCustomization();
        // Act & Assert
        Assert.True(sut.ConfigureMembers);
    }

    [Fact]
    public void CtorThrowsWhenRelayIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AutoConfiguredMockCustomization(null));
    }

    [Fact]
    public void RelayIsMockRelayByDefault()
    {
        // Arrange
        var sut = new AutoConfiguredMockCustomization();
        // Act
        var relay = sut.Relay;
        // Assert
        Assert.IsType<MockRelay>(relay);
    }

    [Fact]
    public void CustomizeThrowsWhenFixtureIsNull()
    {
        // Arrange
        var sut = new AutoConfiguredMockCustomization();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => sut.Customize(null));
    }

    [Fact]
    public void CustomizeAddsPostprocessorToCustomizations()
    {
        // Arrange
        var customizations = new List<ISpecimenBuilder>();
        var fixture = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
        fixture.Setup(f => f.Customizations)
            .Returns(customizations);

        var sut = new AutoConfiguredMockCustomization();
        // Act
        sut.Customize(fixture.Object);
        // Assert
        Assert.Contains(customizations, builder => builder is Postprocessor);
    }

    [Theory]
    [InlineData(typeof(MockVirtualMethodsCommand))]
    [InlineData(typeof(StubPropertiesCommand))]
    [InlineData(typeof(AutoMockPropertiesCommand))]
    public void CustomizeAddsMockCommandsToPostprocessor(Type expectedCommandType)
    {
        // Arrange
        var customizations = new List<ISpecimenBuilder>();
        var fixture = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
        fixture.Setup(f => f.Customizations)
            .Returns(customizations);

        var sut = new AutoConfiguredMockCustomization();
        // Act
        sut.Customize(fixture.Object);
        // Assert
        var postprocessor = (Postprocessor)customizations.Single(builder => builder is Postprocessor);
        var compositeCommand = (CompositeSpecimenCommand)postprocessor.Command;

        Assert.Contains(compositeCommand.Commands, command => command.GetType() == expectedCommandType);
    }

    [Fact]
    public void CustomizeAddsRelayToResidueCollectors()
    {
        // Arrange
        var relay = new Mock<ISpecimenBuilder>();
        var collectors = new List<ISpecimenBuilder>();
        var fixture = new Mock<IFixture> { DefaultValue = DefaultValue.Mock };
        fixture.Setup(f => f.ResidueCollectors)
            .Returns(collectors);

        var sut = new AutoConfiguredMockCustomization(relay.Object);
        // Act
        sut.Customize(fixture.Object);
        // Assert
        Assert.Contains(relay.Object, collectors);
    }
}