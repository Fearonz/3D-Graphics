using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{
    class OctTree
    {
        public float Size { get; set; }
        public Vector3 Position { get; set; }
        public int MaxObjects { get; set; }
        public BoundingBox Bounds { get; set; }

        public List<GameObject3D> Objects { get; set; }
        public List<OctTree> Nodes { get; set; }

        public OctTree(float size, Vector3 position, int maxObjects)
        {
            Size = size;
            Position = position;
            MaxObjects = maxObjects;

            Objects = new List<GameObject3D>();
            Nodes = new List<OctTree>();

            //TODO: create bounds of given size at given position
            float halfSize = size / 2;
            Vector3 min = new Vector3(position.X - halfSize, position.Y - halfSize, position.Z - halfSize);
            Vector3 max = new Vector3(position.X + halfSize, position.Y + halfSize, position.Z + halfSize);
            Vector3 z = new Vector3(position.Z + halfSize, position.Z + halfSize, 0);
            Bounds = new BoundingBox(min, max);

            DebugEngine.AddBoundingBox(Bounds, Color.Red, 1000);
        }
        public void Subdivide()
        {
            //Sub divide the current node’s bounding box into 4 equal boxes
            float childSize = Size / 2;

            OctTree TTL = new OctTree(
                    childSize,
                    new Vector3(Position.X - childSize / 2, Position.Y + childSize / 2, Position.Z - childSize / 2),
                    MaxObjects);

            OctTree TTR = new OctTree(
                  childSize,
                  new Vector3(Position.X + childSize / 2, Position.Y + childSize / 2, Position.Z - childSize / 2),
                  MaxObjects);


            OctTree TBL = new OctTree(
                    childSize,
                    new Vector3(Position.X + childSize / 2, Position.Y + childSize / 2, Position.Z + childSize / 2),
                    MaxObjects);

            OctTree TBR = new OctTree(
                childSize,
                new Vector3(Position.X + childSize / 2, Position.Y - childSize / 2, Position.Z + childSize / 2),
                MaxObjects);


            OctTree BBL = new OctTree(
                    childSize,
                    new Vector3(Position.X - childSize / 2, Position.Y - childSize / 2,Position.Z + childSize / 2),
                    MaxObjects);

            OctTree BBR = new OctTree(
                    childSize,
                    new Vector3(Position.X + childSize / 2, Position.Y - childSize / 2,Position.Z + childSize / 2),
                    MaxObjects);

            OctTree BTL = new OctTree(
                    childSize,
                    new Vector3(Position.X - childSize / 2, Position.Y + childSize / 2, Position.Z - childSize / 2),
                    MaxObjects);

            OctTree BTR = new OctTree(
                    childSize,
                    new Vector3(Position.X - childSize / 2, Position.Y - childSize / 2, Position.Z - childSize / 2),
                    MaxObjects);

            Nodes.Add(TTL);
            Nodes.Add(TTR);
            Nodes.Add(BBL);
            Nodes.Add(BBR);
            Nodes.Add(BTL);
            Nodes.Add(BTR);
            Nodes.Add(TBL);
            Nodes.Add(TBR);
        }
        public void AddObject(GameObject3D newObject)
        {
            if (Nodes.Count == 0)
            {
                if (Objects.Count < MaxObjects)
                {
                    Objects.Add(newObject);
                }
                else
                {
                    Subdivide();

                    foreach (GameObject3D go in Objects)
                        Distribute(go);

                    Objects.Clear();
                }
            }
            else
            {
                Distribute(newObject);
            }
        }

        public void Distribute(GameObject3D newObject)
        {
            Vector3 position = newObject.World.Translation;
            foreach (OctTree node in Nodes)
            {
                if (node.Bounds.Contains(position) != ContainmentType.Disjoint)
                {
                    node.AddObject(newObject);
                    break;
                }
            }
        }

        public void Process(BoundingFrustum frustum, ref List<GameObject3D> foundObjects)
        {
            if (foundObjects == null)
                foundObjects = new List<GameObject3D>();

            if (frustum.Contains(Bounds) != ContainmentType.Disjoint)
            {
                foundObjects.AddRange(Objects);

                foreach (OctTree node in Nodes)
                    node.Process(frustum, ref foundObjects);
            }
        }

    }
}

