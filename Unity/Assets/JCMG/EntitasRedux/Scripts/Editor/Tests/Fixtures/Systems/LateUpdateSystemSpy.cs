using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	public class LateUpdateSystemSpy : ILateUpdateSystem
	{
		public int DidExecute => _didExecute;

		private int _didExecute;

		public void LateUpdate()
		{
			_didExecute += 1;
		}
	}
}
