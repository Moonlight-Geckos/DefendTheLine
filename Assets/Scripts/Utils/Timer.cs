using UnityEngine;
using UnityEngine.Events;

public class Timer
{
	#region Attributes

	bool running = false;
	bool available = false;
	float totalSeconds = 0;
	float elapsedSeconds = 0;

	UnityEvent timerFinishedEvent = new UnityEvent();

	#endregion

	#region Properties

	public float Duration
    {
		set
        {
			if (!running)
            {
				totalSeconds = value;
			}
		}
	}

	public bool Running
    {
		get { return running; }
	}
	public bool Available
	{
		get { return available; }
		set 
		{
			available = value;
			timerFinishedEvent.RemoveAllListeners();
			totalSeconds = 0;
			running = false;
		}
	}

	#endregion

	#region Methods

	public void Update(float t)
	{
		if (!Available)
			return;
		if (running)
		{
			elapsedSeconds += t;
			if (elapsedSeconds >= totalSeconds)
            {
				running = false;
				timerFinishedEvent.Invoke();
			}
		}
	}
	public void Run()
    {
        if (!Available)
        {
			Debug.LogError("Timer runned and not ready");
			return;
        }
		if (totalSeconds > 0)
		{
			running = true;
			elapsedSeconds = 0;
		}
	}
	public void Stop()
    {
		running = false;
	}
	public void AddTimerFinishedEventListener(UnityAction handler)
    {
		timerFinishedEvent.AddListener(handler);
	}
	public void Refresh()
	{
		elapsedSeconds = 0;
	}
	#endregion
}
