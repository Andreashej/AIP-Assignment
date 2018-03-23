using UnityEngine;
using IronPython.Hosting;

public class pytest : MonoBehaviour
{
	void Start()
	{
		var engine = Python.CreateEngine();
		var scope = engine.CreateScope();

		//string code = "str = 'Hello world!'";

		var source = engine.CreateScriptSourceFromFile("Assets/test.py");
		source.Execute(scope);

		Debug.Log(scope.GetVariable<string>("test"));
	}
}