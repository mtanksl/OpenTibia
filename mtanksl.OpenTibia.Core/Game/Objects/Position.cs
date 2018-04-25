namespace OpenTibia
{
    public class Position
    {
        public Position(int x, int y, int z)
        {
            this.x = (ushort)x;

            this.y = (ushort)y;

            this.z = (byte)z;
        }

        private ushort x;

        public ushort X
        {
            get
            {
                return x;
            }
        }

        private ushort y;

        public ushort Y
        {
            get
            {
                return y;
            }
        }

        private byte z;

        public byte Z
        {
            get
            {
                return z;
            }
        }

        public bool IsSlot
        {
            get
            {
                if (x == 65535)
                {
                    if (y < 64)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Slot SlotIndex
        {
            get
            {
                return (Slot)(y);
            }
        }

        public bool IsContainer
        {
            get
            {
                if (x == 65535)
                {
                    if (y >= 64)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public byte ContainerId
        {
            get
            {
                return (byte)(y - 64);
            }
        }

        public byte ContainerIndex
        {
            get
            {
                return (byte)(z);
            }
        }
        
        public bool CanSee(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y + deltaZ;

            int deltaX = that.x - this.x + deltaZ;

            if (this.z >= 8)
            {
                if (deltaZ < -2 || deltaZ > 2)
                {
                    return false;
                }
            }

            if (this.z <= 7)
            {
                if (that.z >= 8)
                {
                    return false;
                }
            }

            if (deltaX < -8 || deltaX > 9 || deltaY <-6 || deltaY > 7)
            {
                return false;
            }

            return true;
        }

        public bool CanHearSay(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaZ != 0 || deltaX < -8 || deltaX > 9 || deltaY < -6 || deltaY > 7)
            {
                return false;
            }

            return true;
        }

        public bool CanHearWhisper(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaZ != 0 || deltaX < -1 || deltaX > 1 || deltaY < -1 || deltaY > 1)
            {
                return false;
            }

            return true;
        }

        public bool CanHearYell(Position that)
        {
            int deltaZ = that.z - this.z;

            int deltaY = that.y - this.y + deltaZ;

            int deltaX = that.x - this.x + deltaZ;

            if (this.z >= 8 || that.z >= 8)
            {
                if (deltaZ != 0)
                {
                    return false;
                }
            }

            if (deltaX < -30 || deltaX > 30 || deltaY < -30 || deltaY > 30)
            {
                return false;
            }

            return true;
        }

        public Position Offset(int a, int b, int c)
        {
            return new Position(this.x + a, this.y + b, this.z + c);
        }

        public Position Offset(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:

                    return Offset(0, -1, 0);

                case Direction.East:

                     return Offset(1, 0, 0);

                case Direction.South:

                     return Offset(0, 1, 0);

                default:

                     return Offset(-1, 0, 0);
            }
        }

        public Position Offset(MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.East:

                    return Offset(1, 0, 0);

                case MoveDirection.NorthEast:

                    return Offset(1, -1, 0);

                case MoveDirection.North:

                    return Offset(0, -1, 0);

                case MoveDirection.NorthWest:

                    return Offset(-1, -1, 0);

                case MoveDirection.West:

                    return Offset(-1, 0, 0);

                case MoveDirection.SouthWest:

                    return Offset(-1, 1, 0);

                case MoveDirection.South:

                    return Offset(0, 1, 0);

                default:

                    return Offset(1, 1, 0);
            }
        }
        
        public Direction ToDirection(Position that)
        {
            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaX < 0)
            {
                return Direction.West;
            }
            else if (deltaX == 0)
            {
                if (deltaY < 0)
                {
                    return Direction.North;
                }
                else if (deltaY > 0)
                {
                    return Direction.South;
                }
            }
            else if (deltaX > 0)
            {
                return Direction.East;
            }

            return Direction.South;
        }

        public MoveDirection ToMoveDirection(Position that)
        {
            int deltaY = that.y - this.y;

            int deltaX = that.x - this.x;

            if (deltaY < 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.NorthWest;
                }
                else if (deltaX == 0)
                {
                    return MoveDirection.North;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.NorthEast;
                }
            }
            else if (deltaY == 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.West;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.East;
                }
            }
            else if (deltaY > 0)
            {
                if (deltaX < 0)
                {
                    return MoveDirection.SouthWest;
                }
                else if (deltaX == 0)
                {
                    return MoveDirection.South;
                }
                else if (deltaX > 0)
                {
                    return MoveDirection.SouthEast;
                }
            }

            return MoveDirection.South;
        }

        public Delta ToDelta(Position that)
        {
            return new Delta(that.x - this.x, that.y - this.y, that.z - this.z);
        }

        public static bool operator ==(Position a, Position b)
        {
            if ( object.ReferenceEquals(a, b) )
            {
                return true;
            }

            if ( object.ReferenceEquals(a, null) || object.ReferenceEquals(b, null) )
            {
                return false;
            }

            return (a.x == b.x) && (a.y == b.y) && (a.z == b.z);
        }

        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }

        public override bool Equals(object position)
        {
            return Equals(position as Position);
        }

        public bool Equals(Position position)
        {
            if (position == null)
            {
                return false;
            }

            return (x == position.x) && (y == position.y) && (z == position.z);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 23 + x.GetHashCode();

            hashCode = hashCode * 23 + y.GetHashCode();

            hashCode = hashCode * 23 + z.GetHashCode();

            return hashCode;
        }

        public override string ToString()
        {
            if (IsSlot)
            {
                return "Slot index: " + SlotIndex;
            }

            if (IsContainer)
            {
                return "Container id: " + ContainerId + " index: " + ContainerIndex;
            }

            return "Coordinate x: " + x + " y: " + y + " z: " + z;
        }
    }
}