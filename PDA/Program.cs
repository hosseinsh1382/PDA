var path = Console.ReadLine();
public class Reader
{
    public async Task<(PDA, List<string>)> Read(string path)
    {
        var context = await File.ReadAllLinesAsync(path);
        var stateCount = Convert.ToInt32(context[2]);
        var transfersCount = Convert.ToInt32(context[3]);
        var transfers = new string[transfersCount];
        for (var i = 0; i < transfersCount; i++)
        {
            transfers[i] = context[4 + i];
        }
        var finalStates = context[transfersCount + 5].Split(" ");
        var inputs = new List<string>();
        var index = transfersCount + 6;
        while (context[index] != "$")
        {
            inputs.Add(context[index]);
            index++;
        }

        return (new PDA( stateCount, transfers, finalStates), inputs);
    }
}

public class PDA
{
    public List<State> States { get; set; } = [];
    private Stack<char> Stack { get; set; } = new();

    public PDA(int statesCount, string[] transfers, string[] finalStates)
    {
        for (var i = 0; i < statesCount; i++)
        {
            var newState = new State
            {
                Name = i.ToString(),
            };
            States.Add(newState);
        }

        foreach (var transfer in transfers)
        {
            var parts = transfer.Split(",");
            var startState = States.SingleOrDefault(x => x.Name == parts[0]);
            if (startState is null)
            {
                throw new ArgumentException();
            }

            var newTransfer = new Transfer
            {
                Input = parts[1],
                Destination = States.Single(x => x.Name == parts[2].Last().ToString()),
                Pop = parts[2].First().ToString(),
                Push = parts.Last()
            };
            startState.Transfers.Add(newTransfer);
        }

        foreach (var finalState in finalStates)
        {
            States.Single(x => x.Name == finalState).IsFinal = true;
        }

        Stack.Push('Z');
    }

    public bool Trace(string input)
    {
        var currentState = States.Single(x => x.Name == "0");
        foreach (var transfer in input.Select(i =>
                     currentState.Transfers.SingleOrDefault(x => x.Input == i.ToString())))
        {
            if (transfer is null)
            {
                return false;
            }

            if (transfer.Pop != Stack.Peek().ToString())
            {
                return false;
            }

            Stack.Pop();
            var push = transfer.Push.Reverse();
            foreach (var p in push)
            {
                if (p != '@')
                {
                    Stack.Push(p);
                }
            }

            currentState = transfer.Destination;
        }

        return currentState.IsFinal;
    }
}

public class State
{
    public string Name { get; set; }
    public List<Transfer> Transfers { get; set; } = [];
    public bool IsFinal { get; set; }
}

public class Transfer
{
    public string Input { get; set; }
    public State Destination { get; set; }
    public string Pop { get; set; }
    public string Push { get; set; }
}