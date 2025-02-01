
using TaskWrapper;

string location = "/home/ff/PycharmProjects/PythonProject/test.py";
//string location = "/home/ff/RiderProjects/PW_Gestione_Punti/Gestionale_Punti/Gestionale_Punti.csproj";
var task = new Wrapper(location, "", 1, true);
await task.Start();