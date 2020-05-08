using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	public class FixedUpdateSystemSpy : IFixedUpdateSystem
	{
		public int DidExecute => _didExecute;

		private int _didExecute;

		public void FixedUpdate()
		{
			_didExecute += 1;
		}
	}
}
