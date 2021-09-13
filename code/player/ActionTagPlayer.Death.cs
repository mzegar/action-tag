using System.Threading.Tasks;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer : Sandbox.Player
	{
		public PlayerCorpse Ragdoll { get; set; }
		private DamageInfo _lastDamageInfo;
		
		public override void OnKilled()
		{
			base.OnKilled();
			
			BecomeRagdollOnServer( _lastDamageInfo.Force, GetHitboxBone( _lastDamageInfo.HitboxIndex ) );

			MakeSpectator();
		}
		
		public void MakeSpectator()
		{
			EnableAllCollisions = false;
			EnableDrawing = false;
			Controller = null;
			Camera = new SpectateRagdollCamera();
			LifeState = LifeState.Dead;
			Health = 0f;

			_ = SwapToFreeCamera( 2000 );
		}

		private async Task SwapToFreeCamera(int ms)
		{
			await Task.Delay(ms);
			if ( IsSpectator )
				Camera = new ActionTagFreeCamera();
		}
		
		private void BecomeRagdollOnServer( Vector3 force, int forceBone )
		{
			var ragdoll = new PlayerCorpse
			{
				Position = Position,
				Rotation = Rotation
			};

			ragdoll.CopyFrom( this );
			ragdoll.ApplyForceToBone( force, forceBone );
			ragdoll.Player = this;

			Ragdoll = ragdoll;
		}
		
		public void RemoveRagdollEntity()
		{
			if ( Ragdoll == null || !Ragdoll.IsValid() )
			{
				return;
			}

			Ragdoll.Delete();
			Ragdoll = null;
		}
	}
}
