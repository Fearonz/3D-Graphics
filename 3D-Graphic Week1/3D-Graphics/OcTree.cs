
using Microsoft.Xna.Framework;
using Sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics
{
    public class OcTree
    {
        public BoundingBox Bounds { get; set; }
        public List<GameObject3D> Objects { get; set; }

        public float Size = 20;
        public Vector3 Center = Vector3.Zero;

        private int MaxNumberOfObjects = 5;

        public List<OcTree> Nodes { get; set; }

        public OcTree(Vector3 position, float size)
        {
            Center = position;
            Size = size;

            Objects = new List<GameObject3D>();
            Nodes = new List<OcTree>();

            var minV2 = Vector3.Subtract(Center, new Vector3(size / 2, size / 2, size / 2));
            var maxV2 = Vector3.Add(Center, new Vector3(size / 2, size / 2, size / 2));

            Bounds = new BoundingBox(
                new Vector3(minV2.X, minV2.Y, minV2.Z),
                new Vector3(maxV2.X, maxV2.Y, maxV2.Z));

            DebugEngine.AddBoundingSphere(new BoundingSphere(Center, 0.5f), Color.Black, 1000);
        }

        public void Clear()
        {
            Objects.Clear();

            foreach (var node in Nodes)
                ClearNode(node);

            Nodes.Clear();
        }

        private void ClearNode(OcTree node)
        {
            if (node == null)
                return;

            node.Clear();
            node = null;
        }

        public void SubDivide()
        {
            float subWidth = ((Bounds.Max - Bounds.Min) / 4).X;
            float subHeight = ((Bounds.Max - Bounds.Min) / 4).Y;
            subHeight = subWidth;

            //upper
            var nodeUFR = new OcTree(new Vector3(Center.X + subWidth, Center.Y - subWidth, Center.Z + subWidth), Size / 2);
            var nodeUFL = new OcTree(new Vector3(Center.X - subWidth, Center.Y - subWidth, Center.Z + subWidth), Size / 2);
            var nodeUBR = new OcTree(new Vector3(Center.X + subWidth, Center.Y - subWidth, Center.Z - subWidth), Size / 2);
            var nodeUBL = new OcTree(new Vector3(Center.X - subWidth, Center.Y - subWidth, Center.Z - subWidth), Size / 2);
            //lower
            var nodeLFR = new OcTree(new Vector3(Center.X + subWidth, Center.Y + subWidth, Center.Z + subWidth), Size / 2);
            var nodeLFL = new OcTree(new Vector3(Center.X - subWidth, Center.Y + subWidth, Center.Z + subWidth), Size / 2);
            var nodeLBR = new OcTree(new Vector3(Center.X + subWidth, Center.Y + subWidth, Center.Z - subWidth), Size / 2);
            var nodeLBL = new OcTree(new Vector3(Center.X - subWidth, Center.Y + subWidth, Center.Z - subWidth), Size / 2);

            //upper
            Nodes.Add(nodeUFR);
            Nodes.Add(nodeUFL);
            Nodes.Add(nodeUBR);
            Nodes.Add(nodeUBL);
            //lower
            Nodes.Add(nodeLFR);
            Nodes.Add(nodeLFL);
            Nodes.Add(nodeLBR);
            Nodes.Add(nodeLBL);

            foreach (var node in Nodes)
            {
                DebugEngine.AddBoundingBox(node.Bounds, Color.Yellow, 1000);
            }
        }

        

        public void AddObject(GameObject3D newObject)
        {
            if (Nodes.Count == 0)
            {
                bool maxObjectReached = Objects.Count > MaxNumberOfObjects;

                if (maxObjectReached)
                {
                    SubDivide();

                    foreach (var obj in Objects)
                        Distrubte(obj);

                    Objects.Clear();
                }
                else
                {
                    Objects.Add(newObject);
                }
            }
            else
            {
                Distrubte(newObject);
            }
        }

        private void Distrubte(GameObject3D newObject)
        {
            var location = newObject.World.Translation;

            foreach (var node in Nodes)
                if (node.Bounds.Contains(location) != ContainmentType.Disjoint)
                    node.AddObject(newObject);
        }

        public void Process(BoundingFrustum frustum, ref List<GameObject3D> passedObjects)
        {
            if (passedObjects == null)
                passedObjects = new List<GameObject3D>();

            var containment = frustum.Contains(Bounds);

            if (containment != ContainmentType.Disjoint)
            {
                foreach (var go in Objects)
                    passedObjects.Add(go);

                foreach (var node in Nodes)
                    node.Process(frustum, ref passedObjects);
            }
        }

        public void GetAllGameObjects(ref List<GameObject3D> passedObjects)
        {
            if (passedObjects == null)
                passedObjects = new List<GameObject3D>();

            foreach (var go in Objects)
            {
                passedObjects.Add(go);
            }

            foreach (var node in Nodes)
                node.GetAllGameObjects(ref passedObjects);
        }

    }
}
