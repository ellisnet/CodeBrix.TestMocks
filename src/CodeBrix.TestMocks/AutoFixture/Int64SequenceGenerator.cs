using System;
using System.Threading;
using CodeBrix.TestMocks.AutoFixture.Kernel;

namespace CodeBrix.TestMocks.AutoFixture; //was previously: namespace AutoFixture;

/// <summary>
/// Creates a sequence of consecutive numbers, starting at 1.
/// </summary>
public class Int64SequenceGenerator : ISpecimenBuilder
{
    private long l;

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <returns>The next number in a consecutive sequence.</returns>
    [Obsolete("Please use the Create(request, context) method instead.")]
    public long Create()
    {
        return Interlocked.Increment(ref this.l);
    }

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <remarks>Obsolete: Please use the <see cref="Create(object, ISpecimenContext)">Create(request, context)</see> method instead.</remarks>
    /// <returns>The next number in a consecutive sequence.</returns>
    [Obsolete("Please use the Create(request, context) method instead.", true)]
    public long CreateAnonymous()
    {
        return this.Create();
    }

    /// <summary>
    /// Creates an anonymous number.
    /// </summary>
    /// <param name="request">The request that describes what to create.</param>
    /// <param name="context">Not used.</param>
    /// <returns>
    /// The next number in a consecutive sequence, if <paramref name="request"/> is a request
    /// for an 64-bit integer; otherwise, a <see cref="NoSpecimen"/> instance.
    /// </returns>
    public object Create(object request, ISpecimenContext context)
    {
        if (!typeof(long).Equals(request))
        {
            return NoSpecimen.Instance;
        }

#pragma warning disable 618
        return this.Create();
#pragma warning restore 618
    }
}