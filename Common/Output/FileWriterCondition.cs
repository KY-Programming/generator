using KY.Generator.Templates;

namespace KY.Generator.Output;

internal class FileWriterCondition : IOutputCache
{
    private readonly IOutputCache output;
    private readonly bool condition;

    public IEnumerable<ICodeFragment> LastFragments => this.output.LastFragments;

    public FileWriterCondition(IOutputCache output, bool condition)
    {
        this.output = output;
        this.condition = condition;
    }

    public IOutputCache Add(string code, bool keepIndent = false)
    {
        if (this.condition)
        {
            this.output.Add(code, keepIndent);
        }
        return this;
    }

    public IOutputCache Add(params ICodeFragment?[] fragments)
    {
        if (this.condition)
        {
            this.output.Add(fragments);
        }
        return this;
    }

    public IOutputCache Add(IEnumerable<ICodeFragment> fragments)
    {
        if (this.condition)
        {
            this.output.Add(fragments);
        }
        return this;
    }

    public IOutputCache Add(IEnumerable<ICodeFragment> fragments, string separator)
    {
        if (this.condition)
        {
            this.output.Add(fragments, separator);
        }
        return this;
    }

    public IOutputCache CloseLine()
    {
        if (this.condition)
        {
            this.output.CloseLine();
        }
        return this;
    }

    public IOutputCache BreakLine()
    {
        if (this.condition)
        {
            this.output.BreakLine();
        }
        return this;
    }

    public IOutputCache BreakLineIfNotEmpty()
    {
        if (this.condition)
        {
            this.output.BreakLineIfNotEmpty();
        }
        return this;
    }

    public IOutputCache UnBreakLine()
    {
        if (this.condition)
        {
            this.output.UnBreakLine();
        }
        return this;
    }

    public IOutputCache Indent()
    {
        if (this.condition)
        {
            this.output.Indent();
        }
        return this;
    }

    public IOutputCache UnIndent()
    {
        if (this.condition)
        {
            this.output.UnIndent();
        }
        return this;
    }

    public IOutputCache StartBlock()
    {
        if (this.condition)
        {
            this.output.StartBlock();
        }
        return this;
    }

    public IOutputCache EndBlock(bool breakLine = true)
    {
        if (this.condition)
        {
            this.output.EndBlock(breakLine);
        }
        return this;
    }

    public IOutputCache If(bool innerCondition)
    {
        if (this.condition)
        {
            return new FileWriterCondition(this, innerCondition);
        }
        return new NoOperationFileWriter(this.output);
    }

    public IOutputCache EndIf()
    {
        return this.output;
    }

    public IOutputCache ExtraIndent(int indents = 1)
    {
        if (this.condition)
        {
            return this.output.ExtraIndent(indents);
        }
        return this;
    }
}
