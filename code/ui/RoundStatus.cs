using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag.UI
{
	public class RoundStatus : Panel
	{
		private readonly Label _currentRound;
		private readonly Label _currentTime;
		
		public RoundStatus()
		{
			StyleSheet.Load("/ui/RoundStatus.scss");
			
			_currentRound = Add.Label( "", "round" );
			_currentTime = Add.Label( "", "time" );
		}

		public override void Tick()
		{
			base.Tick();

			var actionTagGameInstance = ActionTagGame.Instance;
			if ( actionTagGameInstance == null )
			{
				return;
			}
			
			_currentRound.Text = actionTagGameInstance.Round.RoundName;
			_currentTime.Text = actionTagGameInstance.Round.TimeLeftFormatted;
		}
	}
}
