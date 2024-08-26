public class FsmState : IFsmState
{
	public delegate void Entry(FsmSystem fsm);

	public delegate FsmSystem.ACTION Execute(FsmSystem fsm);

	public delegate void Exit(FsmSystem fsm);

	private Entry _entry;

	private Execute _execute;

	private Exit _exit;

	private string _name = string.Empty;

	public string name => _name;

	public FsmState(string stateName, Entry stateEntry = null, Execute stateExecute = null, Exit stateExit = null)
	{
		_name = stateName;
		_entry = stateEntry;
		_execute = stateExecute;
		_exit = stateExit;
	}

	public void entry(FsmSystem fsm)
	{
		if (_entry != null)
		{
			_entry(fsm);
		}
	}

	public FsmSystem.ACTION execute(FsmSystem fsm)
	{
		if (_execute != null)
		{
			return _execute(fsm);
		}
		return FsmSystem.ACTION.END;
	}

	public void exit(FsmSystem fsm)
	{
		if (_exit != null)
		{
			_exit(fsm);
		}
	}
}
