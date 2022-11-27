namespace Recipes.DatabaseEditor.Commands;

public abstract class Command
{
    public IReadOnlyList<string> Path { get; }
    public string Description { get; }

    protected Command(string name, string description) : this(new[] { name }, description)
    {
    }

    protected Command(IEnumerable<string> path, string description)
    {
        Path = path.ToArray();
        Description = description;
    }

    public abstract void Execute(string[] args);
}