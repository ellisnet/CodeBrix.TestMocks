using System.Collections.Generic;

namespace CodeBrix.TestMocks.Tests.AutoFixture.TestTypeFoundation; //was previously namespace TestTypeFoundation;

public class TypeWithIndexer
{
    private readonly Dictionary<string, string> dict;

    public TypeWithIndexer()
    {
        this.dict = new Dictionary<string, string>();
    }

    public string this[string index]
    {
        get
        {
            return this.dict[index];
        }
        set
        {
            this.dict[index] = value;
        }
    }
}