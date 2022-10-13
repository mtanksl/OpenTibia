using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class Pathfinding
    {
        private IMap map;

        public Pathfinding(IMap map)
        {
            this.map = map;
        }

        public bool CanThrow(Position fromPosition, Position toPosition)
        {
            if ( !fromPosition.CanHearSay(toPosition) )
            {
                return false;
            }

            foreach (var position in Bresenham(fromPosition, toPosition) )
            {
                Tile tile = map.GetTile(position);

                if ( tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.BlockProjectile) ) )
                {
                    return false;
                }
            }

            return true;
        }

        public MoveDirection[] GetMoveDirections(Position fromPosition, Position toPosition)
        {
            List<MoveDirection> moveDirections = new List<MoveDirection>();

            if ( fromPosition.CanHearSay(toPosition) )
            {
                Position[] positions = AStar(fromPosition, toPosition, position =>
                {
                    if ( !fromPosition.CanHearSay(position) )
                    {
                        return false;
                    }

                    Tile tile = map.GetTile(position);

                    if (tile == null || tile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || tile.GetCreatures().Any(c => c.Block) )
                    {
                        return false;
                    }

                    return true;
                } );

                for (int i = 0; i < positions.Length - 1 && !positions[i].IsNextTo(toPosition); i++)
                {
                    moveDirections.Add( positions[i].ToMoveDirection(positions[i + 1] ) );
                }
            }

            return moveDirections.ToArray();
        }

        private IEnumerable<Position> Bresenham(Position fromPosition, Position toPosition)
        {
            int x1 = fromPosition.X, 

                y1 = fromPosition.Y,

                x2 = toPosition.X, 

                y2 = toPosition.Y;

            int xi = x1 < x2 ? 1 : -1,

                yi = y1 < y2 ? 1 : -1;

            int dx = Math.Abs(x2 - x1),

                dy = -Math.Abs(y2 - y1);

            int e = dx + dy;

            while (true)
            {
                if (x1 == x2 && y1 == y2)
                {
                    break;
                }

                yield return new Position(x1, y1, fromPosition.Z);

                int e2 = 2 * e;

                if (e2 >= dy)
                {
                    e += dy;

                    x1 += xi;
                }

                if (e2 <= dx)
                {
                    e += dx;

                    y1 += yi;
                }
            }                
        }
        
        private Position[] AStar(Position fromPosition, Position toPosition, Func<Position, bool> callback)
        {
            Stack<Position> positions = new Stack<Position>();

            HashSet<Position> closed = new HashSet<Position>();

            PriorityQueue<Position, Node> open = new PriorityQueue<Position, Node>(n => n.FromPosition);

                open.Enqueue(new Node(fromPosition, toPosition) );

            while (open.Count > 0)
            {
                Node currentNode = open.Dequeue();

                if (currentNode.EstimatedMoves == 0)
                {
                    while (true)
                    {
                        positions.Push(currentNode.FromPosition);

                        if (currentNode.Parent == null)
                        {
                            break;
                        }

                        currentNode = currentNode.Parent;
                    }

                    break;
                }

                closed.Add(currentNode.FromPosition);

                foreach (var moveDirection in new MoveDirection[]
                {
                    MoveDirection.East,
                    MoveDirection.NorthEast,
                    MoveDirection.North,
                    MoveDirection.NorthWest,
                    MoveDirection.West,
                    MoveDirection.SouthWest,
                    MoveDirection.South,
                    MoveDirection.SouthEast
                } )
                {
                    Position nextPosition = currentNode.FromPosition.Offset(moveDirection);

                    if ( !closed.Contains(nextPosition) )
                    {
                        Node nextNode;

                        if ( !open.TryGetValue(nextPosition, out nextNode) )
                        {
                            if ( nextPosition == toPosition || callback(nextPosition) )
                            {
                                int moves = currentNode.Moves + Node.CalculateCost(moveDirection);

                                nextNode = new Node(nextPosition, toPosition)
                                {
                                    Parent = currentNode,

                                    Moves = moves
                                };

                                open.Enqueue(nextNode);
                            }
                        }
                        else
                        {
                            int moves = currentNode.Moves + Node.CalculateCost(moveDirection);

                            if (nextNode.Moves > moves)
                            {
                                nextNode.Parent = currentNode;

                                nextNode.Moves = moves;
                            }
                        }
                    }
                }
            }

            return positions.ToArray();
        }

        private class Node : IComparable<Node>
        {
            public Node(Position fromPosition, Position toPosition)
            {
                FromPosition = fromPosition;

                EstimatedMoves = Node.ChebyshevDistance(fromPosition, toPosition);
            }

            public Position FromPosition { get; }

            public int EstimatedMoves { get; }

            public Node Parent { get; set; }

            public int Moves { get; set; }

            public int Score
            {
                get
                {
                    return EstimatedMoves + Moves;
                }
            }

            public int CompareTo(Node node)
            {
                return Score.CompareTo(node.Score);
            }

            public static int EuclideanDistance(Position fromPosition, Position toPosition)
            {
                return (int)Math.Sqrt( Math.Pow(toPosition.X - fromPosition.X, 2) + Math.Pow(toPosition.Y - fromPosition.Y, 2) );
            }

            public static int ChebyshevDistance(Position fromPosition, Position toPosition)
            {
                return Math.Max(Math.Abs(toPosition.X - fromPosition.X), Math.Abs(toPosition.Y - fromPosition.Y) );
            }

            public static int ManhattanDistance(Position fromPosition, Position toPosition)
            {
                return Math.Abs(toPosition.X - fromPosition.X) + Math.Abs(toPosition.Y - fromPosition.Y);
            }
            
            public static int CalculateCost(MoveDirection direction)
            {
                if (direction == MoveDirection.NorthEast || direction == MoveDirection.NorthWest || direction == MoveDirection.SouthEast || direction == MoveDirection.SouthWest)
                {
                    return 3;
                }

                return 1;
            }
        }
    }
}