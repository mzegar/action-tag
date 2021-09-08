using Sandbox;

namespace ActionTag
{
	public class ActionTagFirstPersonCamera : Camera
	{
		public override void Activated()
		{
			MoveToTarget();
		}

		public override void Update()
		{
			MoveToTarget();

			FieldOfView = 90f;
		}

		private void MoveToTarget()
		{
			Viewer = Local.Pawn;

			if ( Viewer is not Player player )
			{
				return;
			}

			Pos = player.EyePos;
			Rot = player.EyeRot;
		}
	}
}
