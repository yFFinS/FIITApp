namespace Recipes.DatabaseEditor.Commands;

public class HelpCommand : Command
{
    private readonly TextWriter _output;
    private readonly Lazy<IReadOnlyList<Command>> _commands;

    public HelpCommand(TextWriter output, Lazy<IReadOnlyList<Command>> commands)
        : base("help", "Показать справку по командам")
    {
        _output = output;
        _commands = commands;
    }

    public override void Execute(string[] args)
    {
        foreach (var command in _commands.Value)
        {
            _output.WriteLine($"{string.Join(" ", command.Path)} - {command.Description}");
        }
    }
}