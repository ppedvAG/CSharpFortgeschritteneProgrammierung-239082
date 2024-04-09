namespace DelegatesEvents;

/// <summary>
/// Komponente, die eine beliebige Arbeit verrichtet, und ihren Fortschritt per Events zurückgibt
/// </summary>
public class Component
{
	public event Action Start;

	public event Action End;

	public event Action<int> Progress;

	public void Run()
	{
		Start?.Invoke();
		for (int i = 0; i < 10; i++)
		{
			Thread.Sleep(200);
			Progress?.Invoke(i);
		}
		End?.Invoke();
	}
}