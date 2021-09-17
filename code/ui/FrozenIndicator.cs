using Sandbox;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class FrozenIndicator : Panel
	{
		public FrozenIndicator()
		{
			StyleSheet.Load("/ui/FrozenIndicator.scss");

			var frozenIndicator = Add.Panel( "" );
			frozenIndicator.Add.Label( "❄️" );
			frozenIndicator.Add.Label( "Frozen!", "text" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not ActionTagPlayer player )
			{
				return;
			}

			Enabled = player.Controller?.IsFrozen ?? false;
		}
	}
}
