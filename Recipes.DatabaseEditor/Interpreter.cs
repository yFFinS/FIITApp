using System.Text.RegularExpressions;
using Recipes.DatabaseEditor.Commands;

namespace Recipes.DatabaseEditor;

public class Interpreter
{
    private readonly TextReader _input;
    private readonly TextWriter _output;
    private readonly IReadOnlyList<Command> _commands;

    public Interpreter(TextReader input, TextWriter output, IReadOnlyList<Command> commands)
    {
        _input = input;
        _output = output;
        _commands = commands.ToList();
    }

    private static string[] SplitArgs(string input)
    {
        // Split the input into arguments, but ignore spaces inside quotes, using a regex.

        var matches = Regex.Matches(input, @"[\""].+?[\""]|[^ ]+");
        return matches.Select(m => m.Value.Trim('"')).ToArray();
    }

    private (Command? Command, int ArgumentsStart) TryGetCommand(IReadOnlyList<string> input)
    {
        var index = 0;

        var availableCommands = _commands.ToList();

        while (availableCommands.Count > 0 && index < input.Count)
        {
            availableCommands = availableCommands
                .Where(c => c.Path.Count > index && c.Path[index] == input[index])
                .ToList();
            if (availableCommands.Count == 1)
            {
                return (availableCommands[0], index + 1);
            }

            index++;
        }

        return (null, -1);
    }

    public void Run()
    {
        _output.WriteLine("Введите 'help' для получения списка команд. Для выхода введите пустую строку.");

        while (true)
        {
            var input = _input.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            var args = SplitArgs(input);
            var commandName = args[0];

            var (command, argumentsStart) = TryGetCommand(args);
            if (command is null)
            {
                _output.WriteLine($"Неизвестная команда: {commandName}");
                continue;
            }

            command.Execute(args[argumentsStart..]);
        }
    }
}