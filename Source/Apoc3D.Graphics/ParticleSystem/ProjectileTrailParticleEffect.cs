using System;

namespace Apoc3D.Graphics.ParticleSystem
{
    /// <summary>
    /// A particle system that simulates a projectile trail effect.
    /// </summary>
    public class ProjectileTrailParticleEffect : ParticleEffect
    {
        /// <summary>
        /// Creates a particle system that simulates projectile trail effect.
        /// </summary>
        public ProjectileTrailParticleEffect()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            textureName = "smoke";

            maxParticles = 1000;
            particles = new ParticleVertex[maxParticles];

            duration = TimeSpan.FromSeconds(3);
            durationRandomness = 1.5f;

            emitterVelocitySensitivity = 0.1f;

            minHorizontalVelocity = 0;
            maxHorizontalVelocity = 1;

            minVerticalVelocity = -1;
            maxVerticalVelocity = 1;

            minColor = new Color(64, 96, 128, 255);
            maxColor = new Color(255, 255, 255, 128);

            minRotateSpeed = -4;
            maxRotateSpeed = 4;

            minStartSize = 2;
            maxStartSize = 4;

            minEndSize = 5;
            maxEndSize = 15;
        }
    }
}
