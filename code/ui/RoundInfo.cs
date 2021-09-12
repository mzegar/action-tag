using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class RoundInfo : Panel
	{
		private readonly Sandbox.UI.Panel _timeWrapper;
		
		private readonly Label _currentRound;
		private readonly Label _currentTime;
		
		public RoundInfo()
		{
			StyleSheet.Load("/ui/RoundInfo.scss");

			var roundWrapper = Add.Panel( "Round" );
			_currentRound = roundWrapper.Add.Label( "" );

			_timeWrapper = Add.Panel( "Time" );
			_currentTime = _timeWrapper.Add.Label( "" );
		}

		public override void Tick()
		{
			base.Tick();

			_currentRound.Text = ActionTagGame.Instance?.Round.RoundName;
			
			_timeWrapper.SetClass("disabled", ActionTagGame.Instance?.Round.RoundDuration == 0);
			_currentTime.Text = ActionTagGame.Instance?.Round.TimeLeftFormatted;
		}
	}
}
