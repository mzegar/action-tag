using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class RoundInfo : Panel
	{
		private readonly Label _currentRound;
		private readonly Label _currentTime;
		
		public RoundInfo()
		{
			StyleSheet.Load("/ui/RoundInfo.scss");

			var roundWrapper = Add.Panel( "Round" );
			_currentRound = roundWrapper.Add.Label( "" );

			var timerWrapper = Add.Panel( "Timer" );
			_currentTime = timerWrapper.Add.Label( "" );
		}

		public override void Tick()
		{
			base.Tick();

			_currentRound.Text = ActionTagGame.Instance?.Round?.RoundName;

			_currentTime.Text = ActionTagGame.Instance?.Round is not WaitingRound ? 
								$"⏱️ {ActionTagGame.Instance?.Round?.TimeLeftFormatted}" : 
								$"{Client.All.Count} / {ActionTagGameSettings.MinimumPlayers}";
		}
	}
}
