using DefaultEcs;
using Microsoft.Xna.Framework;
using MonoGame.Aseprite.Sprites;
using PillowFight.Shared.Components;

namespace PillowFight.Shared
{
    internal class Helper
    {
        public static void CreateItem(Entity entity, Rectangle hitbox) {
            entity.Set(new ItemStatus());
            entity.Set(new ItemProperties());
            entity.Set(new PositionComponent(hitbox));
            entity.Set(new VelocityComponent());
            entity.Set(new AccelerationComponent());
            entity.Set(new ModifiableComponent<ItemPhysics>());
            entity.Set(new StageCollider());
            entity.Set(new TimedActions());
            entity.Set(new CollisionComponent());
            entity.Set(new RenderModifier());
        }

        public static void CreateCharacter(Entity entity) {
            entity.Set(new CharacterStatus());
            entity.Set(new CharacterProperties());
            entity.Set(new ModifiableComponent<CharacterPhysics>());
            entity.Set(new HealthComponent(10) { 
                OnDeath = e => {
                    e.Get<HolderComponent>().Holding?.Remove<HeldComponent>();
                }
            });
            entity.Set(new HolderComponent());
            entity.Set(new ControlKeys());
            entity.Set(new InputComponent());
        }

        public static void CreatePlayer(Entity entity, int index) {
            entity.Set(new HealthHUD(index));
            if (index == 0) entity.Set(Game1.Camera);
        }

        public static void CreatePillow(Entity pillow)
        {
            var anim = Assets.Aseprites["Cloud"].CreateAnimatedSprite("Cloud");
            anim.Play(0);
            pillow.Set(new AsepriteSprite() { sprites = new AnimatedSprite[] { anim }});

            pillow.Set(new ItemProperties());
            pillow.Set(new ItemStatus());
            pillow.Set(new VelocityComponent());
            pillow.Set(new AccelerationComponent());
            pillow.Set(new ModifiableComponent<ItemPhysics>(new ItemPhysics(){XRestitution = .7f, YRestitution = .5f}));
            pillow.Set(new CollisionComponent());
            // pillow.Set(new Colliders());
            pillow.Set(new HolderComponent());
            pillow.Set(new StageCollider());
            pillow.Set(new TimedActions());
            pillow.Set(new HealthComponent());
            pillow.Set(new Kickable());
            pillow.Set(new Holdable(){ OnHold = PillowOnHold, OnThrow = PillowOnThrow });
            pillow.Set(new RenderModifier());
            pillow.Set(new PillowComponent() { State = Enums.PillowState.Held });
        }

        public static void PillowOnHold(Entity entity) {
            
            entity.Disable<ItemProperties>();
            entity.Disable<ItemStatus>();
            entity.Disable<VelocityComponent>();
            entity.Disable<ModifiableComponent<ItemPhysics>>();
            entity.Disable<CollisionComponent>();
            // pillow.Disable<Colliders>();
            entity.Disable<HolderComponent>();
        }

        public static void PillowOnThrow(Entity pillow) {
            if (pillow.Get<StageCollider>().Colliding) {
                pillow.Remove<ImpulseComponent>();
                pillow.Set(new VelocityComponent());
            }
            pillow.Get<PillowComponent>().State = Enums.PillowState.Stationery;

            pillow.Enable<ItemProperties>();
            pillow.Enable<ItemStatus>();
            pillow.Enable<VelocityComponent>();
            pillow.Enable<ModifiableComponent<ItemPhysics>>();
            pillow.Enable<CollisionComponent>();
            // pillow.Enable<Colliders>();
            pillow.Enable<HolderComponent>();
        }
    }
}
