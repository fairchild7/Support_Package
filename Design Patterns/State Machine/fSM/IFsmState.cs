public interface IFsmState
{
	string name
	{
		get;
	}

	void entry(FsmSystem fsm);

	FsmSystem.ACTION execute(FsmSystem fsm);

	void exit(FsmSystem fsm);
}
