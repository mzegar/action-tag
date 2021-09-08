using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class SpeedInfo : Panel
	{
		private readonly Label _currentSpeed;
		
		public SpeedInfo()
		{
			StyleSheet.Load("/ui/SpeedInfo.scss");
			
			_currentSpeed = Add.Label( "", "speed" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not ActionTagPlayer player )
			{
				return;
			}

			_currentSpeed.Text = $"{player.HorizontalSpeed} u/s";
		}
	}
}
