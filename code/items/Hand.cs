using System.Threading.Tasks;
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
		public virtual int MeleeDistance => 40;

		public override void Spawn()
		{
			base.Spawn();
			
			SetModel( "" );
		}

		// Wait for the "Tag" animation to play before we do the MeleeStrike.
		private async Task DelayMeleeStrike()
		{
			await Task.Delay( 250 );
			MeleeStrike( BaseDamage, 1.5f );
		}

		protected virtual void MeleeStrike( float damage, float force )
		{
			var forward = Owner.EyeRot.Forward;
			forward = forward.Normal;

			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * MeleeDistance, 10f ) )
			{
				if ( !tr.Entity.IsValid() ) continue;

				if ( !IsServer ) continue;
				
				if (tr.Entity is not ActionTagPlayer otherPlayer) continue;

				if (Owner is ActionTagPlayer player && player.Team.Name == otherPlayer.Team.Name) continue;

				using ( Prediction.Off() )
				{
					var damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100 * force, damage )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}
		}

		public override void AttackPrimary()
		{
			(Owner as AnimEntity).SetAnimBool( "b_attack", true );
			ShootEffects();
			_ = DelayMeleeStrike();
		}
		
		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
		}
	}
}
