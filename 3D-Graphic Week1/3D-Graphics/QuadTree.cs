using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class QuadTree
    {
        public float Size { get; set; }
        public Vector2 Position { get; set; }
        public int MaxObjects { get; set; }
        public BoundingBox Bounds { get; set; }

        public List<GameObject3D> Objects { get; set; }
        public List<QuadTree> Nodes { get; set; }

        public QuadTree(float size, Vector2 position, int maxObjects)
        {
            Size = size;
            Position = position;
            MaxObjects = maxObjects;

            Objects = new List<GameObject3D>();
            Nodes = new List<QuadTree>();

            //TODO: create bounds of given size at given position
            float halfSize = size / 2;
            Vector3 min = new Vector3(position.X - halfSize, position.Y - halfSize, 0);
            Vector3 max = new Vector3(position.X + halfSize, position.Y + halfSize, 0);
            Bounds = new BoundingBox(min, max);

            DebugEngine.AddBoundingBox(Bounds, Color.Red, 1000);
            DebugEngine.AddBoundingSphere(new BoundingSphere(new Vector3(position, 0), 1), Color.Black, 1000);
        }

        public void Subdivide()
        {
            //Sub divide the current node’s bounding box into 4 equal boxes
            float childSize = Size / 2;

            QuadTree TL = new QuadTree(
                    childSize, 
                    new Vector2(Position.X - childSize / 2, Position.Y + childSize/2), 
                    MaxObjects);

            QuadTree BL = new QuadTree(
                    childSize,
                    new Vector2(Position.X - childSize / 2, Position.Y - childSize / 2),
                    MaxObjects);

            QuadTree TR = new QuadTree(
                    childSize,
                    new Vector2(Position.X + childSize / 2, Position.Y + childSize / 2),
                    MaxObjects);

            QuadTree BR = new QuadTree(
                    childSize,
                    new Vector2(Position.X + childSize / 2, Position.Y - childSize / 2),
                    MaxObjects);

            Nodes.Add(TL);
            Nodes.Add(TR);
            Nodes.Add(BL);
            Nodes.Add(BR);
        }

        public void AddObject(GameObject3D newObject)
        {
            if(Nodes.Count == 0)
            {
                if(Objects.Count < MaxObjects)
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
            foreach(QuadTree node in Nodes)
            {
                if(node.Bounds.Contains(position) != ContainmentType.Disjoint)
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

            if(frustum.Contains(Bounds) != ContainmentType.Disjoint)
            {
                foundObjects.AddRange(Objects);

                foreach (QuadTree node in Nodes)
                    node.Process(frustum, ref foundObjects);
            }
        }

    }
}
