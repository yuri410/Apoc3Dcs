using System;

namespace VirtualBicycle.Graphics.ParticleSystem
{
    /// <summary>
    /// A particle system that simulates an explosion effect with smoke.
    /// </summary>
    public class ExplosionSmokeParticleEffect : ParticleEffect
    {
        /// <summary>
        /// Creates a particle system that simulates an explosion effect with smoke.
        /// </summary>
        public ExplosionSmokeParticleEffect()
            : base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            textureName = "smoke";

            maxParticles = 200;
            particles = new ParticleVertex[maxParticles];

            duration = TimeSpan.FromSeconds(4);

            minHorizontalVelocity = 0;
            maxHorizontalVelocity = 50;

            minVerticalVelocity = -10;
            maxVerticalVelocity = 50;

            gravity = new Vector3(0, -20, 0);

            endVelocity = 0;

            minColor = Color.LightGray;
            maxColor = Color.White;

            minRotateSpeed = -2;
            maxRotateSpeed = 2;

            minStartSize = 10;
            maxStartSize = 10;

            minEndSize = 100;
            maxEndSize = 200;
        }
    }
}
