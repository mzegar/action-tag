using System.Threading.Tasks;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class Hand : Carriable
	{
		public override string ViewModelPath => "carriables/hand/hand.vmdl";
		public override float PrimaryRate => 1.0f;
		public override float SecondaryRate => 0.3f;
		public override bool IsMelee => true;
		public override int BaseDamage => 10000;
		public virtual int MeleeDistance => 55;

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "" );
		}

		private async Task DelayStrike()
		{
			await Task.Delay( 300 );
			MeleeStrike( BaseDamage, 5f );
		}

		private void MeleeStrike( float damage, float force )
		{
			if ( !IsValid )
			{
				return;
			}

			var forward = Owner.EyeRot.Forward;
			forward = forward.Normal;

			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * MeleeDistance, 10f ) )
			{
				if ( !IsValid ) return;

				if ( !tr.Entity.IsValid() ) continue;

				if ( !IsServer ) continue;

				if ( ActionTagGame.Instance?.Round is not PlayRound ) continue;

				if ( Owner is not ActionTagPlayer player || tr.Entity is not ActionTagPlayer otherPlayer ) return;

				using ( Prediction.Off() )
				{
					switch ( player.Team )
					{
						case ChasersTeam when otherPlayer.Team is RunnerTeam:
							otherPlayer.Controller.IsFrozen = true;
							otherPlayer.OnFrozen();
							break;
						case RunnerTeam when otherPlayer.Team is RunnerTeam && !player.Controller.IsFrozen:
							otherPlayer.Controller.IsFrozen = false;
							break;
					}
				}
			}
		}

		public override void AttackPrimary()
		{
			(Owner as AnimEntity).SetAnimBool( "b_attack", true );
			ShootEffects();
			_ = DelayStrike();
		}

		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
		}
	}
}
