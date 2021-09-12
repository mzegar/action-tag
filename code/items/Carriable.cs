using Sandbox;

namespace ActionTag
{
	partial class Carriable : BaseWeapon
	{
		public virtual bool IsMelee => false;
		public virtual int BaseDamage => 10;
		public override string ViewModelPath => "";
		

		[Net, Predicted]
		public TimeSince TimeSinceDeployed { get; set; }

		public override void ActiveStart( Entity owner )
		{
			base.ActiveStart( owner );

			TimeSinceDeployed = 0;
		}

		public override void Spawn()
		{
			base.Spawn();

			SetModel( "" );
		}
		
		public override void Simulate( Client owner )
		{
			base.Simulate( owner );
		}

		public override void AttackPrimary()
		{
			TimeSincePrimaryAttack = 0;
			TimeSinceSecondaryAttack = 0;

			ShootEffects();
			ShootBullet( 0.05f, 1.5f, BaseDamage, 3.0f );
		}

		[ClientRpc]
		protected virtual void ShootEffects()
		{
			Host.AssertClient();

			if (!IsMelee)
			{
				Particles.Create( "particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle" );
			}

			ViewModelEntity?.SetAnimBool( "tag", true );
		}

		public virtual void ShootBullet( float spread, float force, float damage, float bulletSize )
		{
			var forward = Owner.EyeRot.Forward;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
			forward = forward.Normal;

			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * 5000, bulletSize ) )
			{
				tr.Surface.DoBulletImpact( tr );

				if ( !IsServer ) continue;
				if ( !tr.Entity.IsValid() ) continue;

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

		public override void CreateViewModel()
		{
			Host.AssertClient();

			if ( string.IsNullOrEmpty( ViewModelPath ) )
				return;

			ViewModelEntity = new BaseViewModel {Position = Position, Owner = Owner, EnableViewmodelRendering = true};
			ViewModelEntity.SetModel( ViewModelPath );
		}
	}
}
