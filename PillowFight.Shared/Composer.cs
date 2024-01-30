using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Sprites;
using PillowFight.Shared.Components;

namespace PillowFight.Shared
{
	internal static class Composer
	{

		public static void CreateItem(Entity entity, Rectangle hitbox)
		{
			entity.Set(new ItemStatus());
			entity.Set(new ItemProperties());
			entity.Set(new PositionComponent(hitbox));
			entity.Set(new VelocityComponent());
			entity.Set(new AccelerationComponent());
			entity.Set(new ModifiableComponent<ItemPhysics>());
			entity.Set(new SolidCollider());
			entity.Set(new TimedActions());
			entity.Set(new CollisionComponent());
			entity.Set(new CollisionIgnore());
			entity.Set(new RenderModifier());
		}

		public static void CreateCharacter(Entity entity)
		{
			entity.Set(new CharacterStatus());
			entity.Set(new CharacterProperties());
			entity.Set(new ModifiableComponent<CharacterPhysics>());
			// entity.Set(new HealthComponent(10)
			entity.Set(new HealthComponent()
			{
				Health = 10,
				MaxHealth = 10,
				OnDeath = e => e.Get<HolderComponent>().Holding?.Remove<HeldComponent>()
			});
			entity.Set(new HolderComponent());
			entity.Set(new ControlKeys());
			entity.Set(new InputComponent());
		}

		public static void CreatePlayer(Entity entity, int index)
		{
			entity.Set(new Solid());
			entity.Set(new HealthHUD(index)
			{
				Scale = new Vector2(2, 2),
				Position = new Vector2(128, 0)
			});
			if (index == 0) entity.Set(Game1.Camera);
		}

		public static void CreatePillow(Entity pillow)
		{
			// var anim = Assets.Aseprites["Cloud"].CreateAnimatedSprite("Cloud");
			// anim.Play(0);
			// pillow.Set(new AsepriteSprite() { sprites = new AnimatedSprite[] { anim } });
			pillow.Set(Assets.Images["Cloud"]);
			pillow.Set(new HealthComponent());
			pillow.Set(new Kickable());
			pillow.Set(new Holdable() { OnHold = Helper.PillowOnHold, OnThrow = Helper.PillowOnThrow });
			pillow.Set(new PillowComponent() { State = Enums.PillowState.Held });
			pillow.Set(new Solid());
		}

		public static void CreateFreezePillow(Entity pillow, World world) {
			pillow.Get<HealthComponent>().OnDeath = (e) => Helper.ForAllNear(world.GetEntities().With<CharacterProperties>(), e.Get<PositionComponent>().Position, 32, e, 
			(p, c) => {
				c.Disable<VelocityComponent>();
				c.Get<TimedActions>().Add(e => e.Set(new VelocityComponent()), 4);
			});
		}

		public static void CreateFloatingPlatform(Entity entity, Rectangle hitbox, bool horizontal = true, bool vertical = false)
		{
			// entity.Remove<AccelerationComponent>();
			// entity.Remove<SolidCollider>();
			entity.Set(new PositionComponent(hitbox));
			entity.Set(new VelocityComponent());
			entity.Set(new RenderModifier());
			entity.Set(new ItemStatus());
			entity.Set(new Solid());

			if (horizontal)
				entity.Set(new SineGenerator(.5f, 2)
				{
					Target = (e, f) =>
					{
						e.Get<VelocityComponent>().X = f;
					}
				});

			if (vertical)
				entity.Set(new SineGenerator(0.5f, 2)
				{
					Target = (e, f) =>
					{
						e.Get<VelocityComponent>().Y = f;
					}
				});
		}

		// public static void SetColliders(Vector2)
	}
}
