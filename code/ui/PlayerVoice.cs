using Sandbox;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class PlayerVoice : Panel
	{
		public PlayerVoice()
		{
			StyleSheet.Load("/ui/PlayerVoice.scss");

			var voiceEmoji = Add.Label( "🔊" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not ActionTagPlayer )
			{
				return;
			}

			SetClass("fadein", !Input.Down( InputButton.Voice ));
		}
	}
}
