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

		private async Task DelayStrike()
		{
			await Task.Delay( 300 );
			MeleeStrike(BaseDamage, 5f);
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
			_ = DelayStrike();
		}
		
		public override void SimulateAnimator( PawnAnimator anim )
		{
			anim.SetParam( "holdtype", 4 );
			anim.SetParam( "holdtype_handedness", 1 );
		}
	}
}
