using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Graphics
{
    class QuadTree
    {
        public float Size { get; set; }
        public Vector2 Position { get; set; }
        public int MaxObjects { get; set; }
        public BoundingBox Bounds { get; set; }
        public List<GameObject3D> Objects { get; set; }
        public List<QuadTree> Nodes { get; set; }

        public QuadTree(float size,Vector2 position, int maxObjects)
        {
            Size = size;
            Position = position;
            maxObjects = MaxObjects;

            Objects = new List<GameObject3D>();
            Nodes = new List<QuadTree>();

            //create bounds of given size
            float halfSize = size / 2;
            Vector3 min = new Vector3(position.X - halfSize, position.Y- halfSize, 0);
            Vector3 max = new Vector3(position.X + halfSize, position.Y + halfSize, 0);
            
            Bounds = new BoundingBox(min, max);
            DebugEngine.AddBoundingBox(Bounds, Color.Red, 1000);
            DebugEngine.AddBoundingSphere(new BoundingSphere(new Vector3(position, 0), 1), Color.Black, 1000);
        }

        public void SubDivide()
        {
            float quadHalf = Size / 2;

            QuadTree TL = new QuadTree(quadHalf, new Vector2(Position.X - quadHalf / 2, Position.Y + quadHalf / 2), MaxObjects);
            QuadTree BL = new QuadTree(quadHalf, new Vector2(Position.X - quadHalf / 2, Position.Y - quadHalf / 2), MaxObjects); 
            QuadTree TR = new QuadTree(quadHalf, new Vector2(Position.X + quadHalf / 2, Position.Y + quadHalf / 2), MaxObjects);
            QuadTree BR = new QuadTree(quadHalf, new Vector2(Position.X + quadHalf / 2, Position.Y - quadHalf / 2), MaxObjects);

            Nodes.Add(TL);
            Nodes.Add(BL);
            Nodes.Add(TR);
            Nodes.Add(BR);
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
                    SubDivide();
                    foreach (GameObject3D go in Objects)
                    {
                        Distribute(go);
                    }
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
            foreach (var node in Nodes)
            {
                if (Bounds.Contains(newObject.World.Translation) != ContainmentType.Disjoint)
                {
                    node.AddObject(newObject);
                    break;
                }
            }
        }

        public void ProcessTree(BoundingFrustum frustum, ref List<GameObject3D> passedObjects)
        {
            if (passedObjects == null)
                passedObjects = new List<GameObject3D>();

            if (frustum.Contains(Bounds)!= ContainmentType.Disjoint)
            {
                passedObjects.AddRange(Objects);

                foreach (QuadTree node in Nodes)
                    ProcessTree(frustum, ref passedObjects);
            }
        }

        public void ClearNoid(QuadTree node)
        {
            Nodes.Clear();
        }
    }
}
