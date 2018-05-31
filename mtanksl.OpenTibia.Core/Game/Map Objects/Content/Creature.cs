using OpenTibia.Common.Objects;
using OpenTibia.Threading;
using System.Linq;

namespace OpenTibia
{
    public abstract class Creature : IContent
    {
        public Creature()
        {
            Health = 100;

            MaxHealth = 100;

            Direction = Direction.South;

            Light = new Light(0, 0);

            Outfit = new Outfit(266, 0, 0, 0, 0, Addons.None);

            Speed = 220;

            DiagonalDelay = 1;

            Skull = Skull.None;

            Party = Party.None;

            War = War.None;

            Block = true;
        }

        public uint Id { get; set; }

        public string Name { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }
        
        public Direction Direction { get; set; }

        public Light Light { get; set; }

        public Outfit Outfit { get; set; }

        public ushort Speed { get; set; }

        public int DiagonalDelay { get; set; }

        public int WalkDelay
        {
            get
            {
                return (1000 * DiagonalDelay * ( (Tile)Container ).Ground.Metadata.Speed) / Speed;
            }
        }

        public Skull Skull { get; set; }

        public Party Party { get; set; }

        public War War { get; set; }
        
        public bool Block { get; set; }
        
        public TopOrder TopOrder
        {
            get
            {
                return TopOrder.Creature;
            }
        }
        
        public IContainer Container { get; set; }

        public Tile Tile
        {
            get
            {
                return Container as Tile;
            }
        }
        
        public SchedulerEvent WalkSchedulerEvent { get; set; }

        public bool CanMove(MoveDirection moveDirection)
        {
            Tile toTile = Game.Current.Map.GetTile(Tile.Position.Offset(moveDirection));

            if (toTile == null || !toTile.Walkable || toTile.GetCreatures().Any())
            {
                return false;
            }

            return true;
        }

        public void Move(MoveDirection moveDirection)
        {
            //Store game state

            Tile fromTile = Tile;

            Position fromPosition = fromTile.Position;

            Position toPosition = fromPosition.Offset(moveDirection);

            Tile toTile = Game.Current.Map.GetTile(toPosition);

            //Change game state

            byte fromIndex = (byte)fromTile.RemoveContent(this);

            byte toIndex = (byte)toTile.AddContent(this);

            this.DiagonalDelay = fromPosition.ToMoveDirection(toPosition).IsDiagonal() ? 2 : 1;

            this.Direction = fromPosition.ToDirection(toPosition);

            //Notify

            Game.Current.EventBus.Publish(new CreatureMoveEvent(this, fromTile, fromIndex, toTile, toIndex));
        }

        public void ChangeHealth(ushort toHealth)
        {
            if (toHealth == 0)
            {
                Tile fromTile = Tile;

                byte fromIndex = (byte)fromTile.RemoveContent(this);

                Game.Current.Map.RemoveCreature(this);

                Game.Current.EventBus.Publish(new CreatureRemoveEvent(this, fromTile, fromIndex));

                Game.Current.EventBus.Publish(new MagicEffectEvent(fromTile.Position, MagicEffectType.Puff));
            }
            else
            {
                ushort fromHealth = Health;

                Health = toHealth;

                Game.Current.EventBus.Publish( new CreatureChangeHealthEvent(this, fromHealth, toHealth) );
            }
        }

        public void Turn(Direction toDirection)
        {
            Direction fromDirection = Direction;

            Direction = toDirection;

            Game.Current.EventBus.Publish( new CreatureTurnEvent(this, fromDirection, toDirection) );
        }

        public void ChangeOutfit(Outfit toOutfit)
        {
            Outfit fromOutfit = Outfit;

            Outfit = toOutfit;

            Game.Current.EventBus.Publish( new CreatureChangeOutfitEvent(this, fromOutfit, toOutfit) );
        }
    }
}