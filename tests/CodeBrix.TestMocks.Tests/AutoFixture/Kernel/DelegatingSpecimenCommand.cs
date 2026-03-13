using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.Tests.AutoFixture.Kernel; //was previously: namespace AutoFixtureUnitTest.Kernel;

public class DelegatingSpecimenCommand : ISpecimenCommand
{
    public DelegatingSpecimenCommand()
    {
        this.OnExecute = (s, c) => { };
    }

    public void Execute(object specimen, ISpecimenContext context)
    {
        this.OnExecute(specimen, context);
    }

    internal Action<object, ISpecimenContext> OnExecute { get; set; }
}