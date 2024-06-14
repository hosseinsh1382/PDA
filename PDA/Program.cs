var path = Console.ReadLine();



public class Reader
{
    public async Task Read(string path)
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

       // return (new PDA( stateCount, transfers, finalStates), inputs);
    }
}

