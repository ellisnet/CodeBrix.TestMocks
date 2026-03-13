using System;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.AutoFixture; //was previously: namespace AutoFixture;

/// <summary>
/// Creates new <see cref="Guid"/> instances.
/// </summary>
public class GuidGenerator : ISpecimenBuilder
{
    /// <summary>
    /// Creates a new <see cref="Guid"/> instance.
    /// </summary>
    [Obsolete("Please use the Create(request, context) method instead.")]
    public static Guid Create()
    {
        return Guid.NewGuid();
    }

    /// <summary>
    /// Creates a new <see cref="Guid"/> instance.
    /// </summary>
    [Obsolete("Please use the Create(request, context) method instead.", true)]
    public static Guid CreateAnonymous()
    {
        return Create();
    }

    /// <inheritdoc />
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(Guid).Equals(request))
        {
            return NoSpecimen.Instance;
        }

#pragma warning disable 618
        return Create();
#pragma warning restore 618
    }
}