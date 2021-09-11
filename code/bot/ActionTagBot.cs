using Sandbox;

namespace ActionTag
{
	public class ActionTagBot : Bot
	{
		[AdminCmd("actiontag_bot_add", Help = "Spawn a bot player that stands around.")]
		static void SpawnActionTagBot()
		{
			new ActionTagBot();
		}
	}
}
