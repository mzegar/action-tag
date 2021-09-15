using ActionTag;

namespace ActionTag
{
	public class Crosshair : Panel
	{
		public Crosshair()
		{
			StyleSheet.Load( "/ui/Crosshair.scss" );
			
			Add.Panel( "Crosshair" );
		}
	}
}
