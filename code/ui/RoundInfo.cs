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
			
			_currentRound = Add.Label( "", "round" );
			_currentTime = Add.Label( "", "time" );
		}

		public override void Tick()
		{
			base.Tick();

			_currentRound.Text = ActionTagGame.Instance?.Round.RoundName;
			_currentTime.Text = ActionTagGame.Instance?.Round.TimeLeftFormatted;
		}
	}
}
