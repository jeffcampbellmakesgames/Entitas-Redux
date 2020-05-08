using JCMG.EntitasRedux;

namespace EntitasRedux.Tests
{
	public class UpdateSystemSpy : IUpdateSystem
	{
		public int DidExecute => _didExecute;

		private int _didExecute;

		public void Update()
		{
			_didExecute += 1;
		}
	}
}
