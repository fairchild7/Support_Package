using System.Collections.Generic;

public class FsmSystem
{
	public enum ACTION
	{
		END,
		CONTINUE
	}

	public const int INVALID_INDEX = -1;

	private IFsmState[] _states;

	private Dictionary<string, int> _indexTbl = new Dictionary<string, int>();

	private int _stateCnt;

	private int _curIndex = -1;

	private int _nextIndex = -1;

	private bool _isEntry;

	public int curStateIndex => _curIndex;

	public void init(IFsmState[] stateTbl)
	{
		if (stateTbl == null || stateTbl.Length <= 0)
		{
			return;
		}
		clearAll();
		_states = stateTbl;
		_curIndex = 0;
		_nextIndex = 0;
		for (int i = 0; i < _states.Length; i++)
		{
			IFsmState fsmState = _states[i];
			if (fsmState != null)
			{
				_indexTbl[fsmState.name] = i;
			}
		}
	}

	public void init(int capacity)
	{
		if (capacity > 0)
		{
			clearAll();
			_states = new IFsmState[capacity];
			_curIndex = 0;
			_nextIndex = 0;
		}
	}

	public int add(IFsmState state)
	{
		if (_states == null || _stateCnt >= _states.Length)
		{
			return -1;
		}
		int stateCnt = _stateCnt;
		_states[stateCnt] = state;
		_indexTbl[state.name] = stateCnt;
		_stateCnt++;
		return stateCnt;
	}

	public void clearAll()
	{
		if (_states != null)
		{
			if (_isEntry && _curIndex >= 0 && _curIndex < _states.Length)
			{
				_states[_curIndex]?.exit(this);
			}
			_indexTbl.Clear();
			_states = null;
			_stateCnt = 0;
			_curIndex = -1;
			_nextIndex = -1;
			_isEntry = false;
		}
	}

	public void execute()
	{
		if (_states == null || _curIndex < 0 || _states.Length <= _curIndex)
		{
			return;
		}
		IFsmState fsmState = _states[_curIndex];
		if (fsmState == null)
		{
			return;
		}
		ACTION aCTION = ACTION.CONTINUE;
		while (true)
		{
			if (_isEntry && _curIndex != _nextIndex)
			{
				fsmState.exit(this);
				_curIndex = _nextIndex;
				_isEntry = false;
			}
			if (aCTION != ACTION.CONTINUE || _states == null || _curIndex < 0 || _states.Length <= _curIndex)
			{
				break;
			}
			fsmState = _states[_curIndex];
			if (fsmState == null)
			{
				break;
			}
			if (!_isEntry)
			{
				fsmState.entry(this);
				_isEntry = true;
			}
			aCTION = ((_curIndex == _nextIndex) ? fsmState.execute(this) : ACTION.END);
		}
	}

	public void setState(string name)
	{
		int value;
		if (_states != null && _indexTbl.TryGetValue(name, out value) && value >= 0 && value < _states.Length)
		{
			_nextIndex = value;
		}
	}

	public void setState(int index)
	{
		if (_states != null && index >= 0 && index < _states.Length)
		{
			_nextIndex = index;
		}
	}

	public void next()
	{
		int num = _nextIndex + 1;
		if (num >= 0 && num < _states.Length)
		{
			_nextIndex = num;
		}
	}

	public bool isState(string name)
	{
		if (_states == null || !_indexTbl.TryGetValue(name, out int value))
		{
			return false;
		}
		return _curIndex == value;
	}

	public bool isState(int index)
	{
		return _curIndex == index;
	}
}
