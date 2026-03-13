using CodeBrix.TestMocks.AutoFixture;
using CodeBrix.TestMocks.AutoFixture.AutoMock;
using CodeBrix.TestMocks.AutoFixture.Kernel;
using System;
using System.Linq;
using Xunit;

namespace CodeBrix.TestMocks.Tests.AutoFixture.AutoMock; //was previously: namespace AutoFixture.AutoMoq.UnitTest;

public class AutoMockCustomizationTest
{
    [Fact]
    public void SutIsCustomization()
    {
        // Arrange
        // Act
        var sut = new AutoMockCustomization();
        // Assert
        Assert.IsAssignableFrom<ICustomization>(sut);
    }

    [Fact]
    public void ConfigureMembersIsDisabledByDefault()
    {
        // Arrange
        // Act
        var sut = new AutoMockCustomization();
        // Assert
        Assert.False(sut.ConfigureMembers);
    }

    [Fact]
    public void GenerateDelegatesIsDisabledByDefault()
    {
        // Arrange
        // Act
        var sut = new AutoMockCustomization();
        // Assert
        Assert.False(sut.GenerateDelegates);
    }

    [Fact, Obsolete]
    public void InitializeWithNullRelayThrows()
    {
        // Arrange
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            new AutoMockCustomization(null));
    }

    [Fact]
    public void ThrowsIfNullRelayIsSet()
    {
        // Arrange
        var sut = new AutoMockCustomization();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Relay = null);
    }

    [Fact]
    public void ShouldPreserveTheSetRelay()
    {
        // Arrange
        var sut = new AutoMockCustomization();
        var relay = new CompositeSpecimenBuilder();
        // Act
        sut.Relay = relay;
        // Assert
        Assert.Equal(relay, sut.Relay);
    }

    [Fact, Obsolete]
    public void SpecificationIsCorrectWhenInitializedWithRelay()
    {
        // Arrange
        var expectedRelay = new MockRelay();
        var sut = new AutoMockCustomization(expectedRelay);
        // Act
        ISpecimenBuilder result = sut.Relay;
        // Assert
        Assert.Equal(expectedRelay, result);
    }

    [Fact]
    public void SpecificationIsNotNullWhenInitializedWithDefaultConstructor()
    {
        // Arrange
        var sut = new AutoMockCustomization();
        // Act
        var result = sut.Relay;
        // Assert
        Assert.IsType<MockRelay>(result);
    }

    [Fact]
    public void CustomizeNullFixtureThrows()
    {
        // Arrange
        var sut = new AutoMockCustomization();
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            sut.Customize(null));
    }

    [Fact]
    public void CustomizeAddsAppropriateResidueCollector()
    {
        // Arrange
        var fixtureStub = new FixtureStub();

        var sut = new AutoMockCustomization();
        // Act
        sut.Customize(fixtureStub);
        // Assert
        Assert.Contains(sut.Relay, fixtureStub.ResidueCollectors);
    }

    [Fact]
    public void CustomizeAddsAppropriateCustomizations()
    {
        // Arrange
        var fixtureStub = new FixtureStub();

        var sut = new AutoMockCustomization();
        // Act
        sut.Customize(fixtureStub);
        // Assert
        var postprocessor = fixtureStub.Customizations.OfType<MockPostprocessor>().Single();
        var ctorInvoker = Assert.IsAssignableFrom<MethodInvoker>(postprocessor.Builder);
        Assert.IsAssignableFrom<MockConstructorQuery>(ctorInvoker.Query);
    }

    [Fact]
    public void WithConfigureMembers_CustomizeAddsPostprocessorToCustomizations()
    {
        // Arrange
        var fixtureStub = new FixtureStub();
        var sut = new AutoMockCustomization { ConfigureMembers = true };
        // Act
        sut.Customize(fixtureStub);
        // Assert
        Assert.Contains(fixtureStub.Customizations, builder => builder is Postprocessor);
    }

    [Theory]
    [InlineData(typeof(MockVirtualMethodsCommand))]
    [InlineData(typeof(StubPropertiesCommand))]
    [InlineData(typeof(AutoMockPropertiesCommand))]
    public void WithConfigureMembers_CustomizeAddsMockCommandsToPostprocessor(Type expectedCommandType)
    {
        // Arrange
        var fixtureStub = new FixtureStub();
        var sut = new AutoMockCustomization { ConfigureMembers = true };
        // Act
        sut.Customize(fixtureStub);
        // Assert
        var postprocessor = (Postprocessor)fixtureStub.Customizations.Single(builder => builder is Postprocessor);
        var compositeCommand = (CompositeSpecimenCommand)postprocessor.Command;

        Assert.Contains(compositeCommand.Commands, command => command.GetType() == expectedCommandType);
    }

    [Fact]
    public void WithGenerateDelegates_CustomizeAddsRelay()
    {
        // Arrange
        var fixtureStub = new FixtureStub();
        var sut = new AutoMockCustomization { GenerateDelegates = true };
        // Act
        sut.Customize(fixtureStub);
        // Assert
        var mockRelay = (MockRelay)fixtureStub.Customizations.Single(c => c is MockRelay);
        Assert.IsType<DelegateSpecification>(mockRelay.MockableSpecification);
    }

    [Fact]
    public void WithoutGenerateDelegates_DoesNotAddMockRelayForDelegate()
    {
        // Arrange
        var fixtureStub = new FixtureStub();
        var sut = new AutoMockCustomization { GenerateDelegates = false };
        // Act
        sut.Customize(fixtureStub);
        // Assert
        Assert.DoesNotContain(fixtureStub.Customizations, c => c is MockRelay);
    }
}