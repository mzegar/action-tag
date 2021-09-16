using Sandbox;

namespace ActionTag
{
	public class Crosshair : Panel
	{
		public Crosshair()
		{
			StyleSheet.Load( "/ui/Crosshair.scss" );
			
			Add.Panel( "Crosshair" );
		}

		public override void Tick()
		{
			base.Tick();

			Enabled = !Input.Down( InputButton.Score );
		}
	}
}
