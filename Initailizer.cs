using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using RiverRideGame;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace RiverRideGame
{
    public class World
    {
        public Rectangle[] Collisions;
        public Rectangle[] Fuels;
        public Rectangle[] Bridges;
        public List<SceneElement> SceneElements;
        public int offsetY;
        public int MoveSpeed = 20;
        public bool EndOfScrolling = false;

        public SceneElement getEnemy(string id)
        {
            foreach (SceneElement e in SceneElements)
                if (e.id == id)
                    return e;
            return null;
        }
    }

    public class Initailizer
    {
        public World initWorld(string mapFile)
        {
            World w = new World();
            w.SceneElements = new List<SceneElement>();

            List<Rectangle> rectCollisions = new List<Rectangle>();
            List<Rectangle> fuels = new List<Rectangle>();
            List<Rectangle> bridges = new List<Rectangle>();

            XmlTextReader xmlReader = new XmlTextReader(mapFile);

            while (xmlReader.Read())
            {
                xmlReader.MoveToElement();
                if (xmlReader.Name == "area")
                {
                    string tmpv = "";
                    if (xmlReader["nohref"] == "nohref")
                    {
                        string[] ctmp = xmlReader["coords"].Split(',');
                        Rectangle rtLeftSide = new Rectangle(int.Parse(ctmp[0]), int.Parse(ctmp[1]), int.Parse(ctmp[2]) - int.Parse(ctmp[0]), int.Parse(ctmp[3]) - int.Parse(ctmp[1]));
                        rectCollisions.Add(rtLeftSide);

                        Rectangle rtRightSide = new Rectangle()
                        {
                            X = RiverRide.SCREEN_WIDTH -  rtLeftSide.X - rtLeftSide.Width,
                            Y = rtLeftSide.Y,
                            Height = rtLeftSide.Height,
                            Width = rtLeftSide.Width
                        };
                        rectCollisions.Add(rtRightSide);
                    } 
                    if ((tmpv = xmlReader["href"]) != null)
                    {                        
                        string[] ctmp = xmlReader["coords"].Split(',');
                        Rectangle rt = new Rectangle(int.Parse(ctmp[0]), int.Parse(ctmp[1]), int.Parse(ctmp[2]) - int.Parse(ctmp[0]), int.Parse(ctmp[3]) - int.Parse(ctmp[1]));

                        if (tmpv.StartsWith("enemyt"))
                        {
                            string[] tmp = tmpv.Split('_');
                            SceneElement e = null;
                            string id = "";
                            if ((e = w.getEnemy(id = tmp[0] + tmp[1])) == null)                           
                            {
                                switch (tmp[0])
                                {
                                    case "enemyt1":
                                        e = new ShipEnemy(new Texture2D[] 
                                        {   
                                            RiverRide.ShipEnemyTexture1,
                                            RiverRide.ShipEnemyTexture2
                                        }, id, w);
                                        break;
                                    case "enemyt2":
                                        e = new HellicopterEnemy(new Texture2D[] 
                                        {   
                                            RiverRide.HelicopterEnemyTexture1,
                                            RiverRide.HelicopterEnemyTexture2
                                        }, id, w);
                                        break;
                                    case "enemyt3":
                                        e = new TankEnemy(new Texture2D[] 
                                        {
                                            RiverRide.TankEnemyTexture1,
                                            RiverRide.TankEnemyTexture2
                                        }, id, w);
                                        break;
                                    case "enemyt4":
                                        e = new Su22Enemy(new Texture2D[] 
                                        {   
                                            RiverRide.Su22EnemyTexture1,
                                            RiverRide.Su22EnemyTexture2
                                        }, id, w);
                                        break;
                                }                                
                                w.SceneElements.Add(e);
                            }
                            if (tmp[2] == "start")
                            {
                                e.RectStart = rt;
                                e.RectPosition = rt;
                                if (tmp.Length == 4)
                                {
                                    if (tmp[3] == "onbridge")
                                        (e as TankEnemy).IsOnBridge = true;
                                }
                            }

                            else if (tmp[2] == "end")
                                e.RectEnd = rt;
                        }
                        if (tmpv == "island")
                        {                            
                            rectCollisions.Add(new Rectangle(int.Parse(ctmp[0]), int.Parse(ctmp[1]), int.Parse(ctmp[2]) - int.Parse(ctmp[0]), int.Parse(ctmp[3]) - int.Parse(ctmp[1])));
                        }
                        if (tmpv == "bridge")
                        {
                            Bridge b = new Bridge(new Texture2D[] 
                                        {   
                                            RiverRide.Bridge,
                                            RiverRide.Bridge
                                        }, "", w);
                            b.RectStart = rt;
                            b.RectPosition = rt;
                            w.SceneElements.Add(b);
                        }
                        if (tmpv == "fuel")
                        {
                            Rectangle rtf = new Rectangle(int.Parse(ctmp[0]), int.Parse(ctmp[1]), int.Parse(ctmp[2]) - int.Parse(ctmp[0]), int.Parse(ctmp[3]) - int.Parse(ctmp[1]));
                            fuels.Add(new Rectangle()
                                {
                                    X = rtf.X,
                                    Y = rtf.Y,
                                    Width = RiverRide.FuelTexture.Width,
                                    Height = RiverRide.FuelTexture.Height
                                });
                        }
                    }
                }
            }
            xmlReader.Close();
            w.Collisions = rectCollisions.ToArray<Rectangle>();
            Array.Sort(w.Collisions, new RectrangleComparer());
            w.Bridges = bridges.ToArray<Rectangle>();
            w.Fuels = fuels.ToArray<Rectangle>();
            
            return w;
        }
    }

    class RectrangleComparer : IComparer<Rectangle>
    {
        #region IComparer Members

        public int Compare(Rectangle x, Rectangle y)
        {
            if (x.Y > y.Y)
                return 1;
            else if (x.Y < y.Y)
                return -1;
            else
                return 0;
        }

        #endregion
    }
}
