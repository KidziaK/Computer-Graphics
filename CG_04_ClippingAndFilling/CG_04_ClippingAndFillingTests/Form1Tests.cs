using Microsoft.VisualStudio.TestTools.UnitTesting;
using CG_04_ClippingAndFilling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static CG_04_ClippingAndFilling.Form1;

namespace CG_04_ClippingAndFilling.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void OrderTest()
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(10, 10));
            points.Add(new Point(10, 16));
            points.Add(new Point(16, 20));
            points.Add(new Point(28, 10));
            points.Add(new Point(28, 16));
            points.Add(new Point(22, 10));


            int len = points.Count;
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < len - 1; i++)
            {
                int _ymin, _ymax, _x;
                float _m;
                int _y1 = points[i].Y;
                int _y2 = points[i + 1].Y;
                int _x1 = points[i].X;
                int _x2 = points[i + 1].X;
                if (_y1 > _y2)
                {
                    _ymin = _y2;
                    _ymax = _y1;
                    _x = _x2;
                }
                else
                {
                    _ymin = _y1;
                    _ymax = _y2;
                    _x = _x1;
                    
                }

                _m = (float)((float)(_x2 - _x1) / (float)(_y2 - _y1));




                if ((_y2 - _y1) != 0)
                {
                    edges.Add(new Edge(_ymin, _ymax, _x, _m));

                }
                else
                {
                    edges.Add(new Edge(_ymin, _ymax, _x, Int32.MaxValue));
                }
            }

            int ymin, ymax, x;
            float m;
            int y1 = points[len-1].Y;
            int y2 = points[0].Y;
            int x1 = points[len-1].X;
            int x2 = points[0].X;
            if (y1 > y2)
            {
                ymin = y2;
                ymax = y1;
                x = x2;
            }
            else
            {
                ymin = y1;
                ymax = y2;
                x = x1;
            }

            m = (float)((float)(x2 - x1) / (float)(y2 - y1));



            if ((y2 - y1) != 0)
            {
                edges.Add(new Edge(ymin, ymax, x, m));
            }
            else
            {
                edges.Add(new Edge(ymin, ymax, x, Int32.MaxValue));
            }

            Console.WriteLine("Before Sorting:");
            Console.WriteLine("ymin \t ymax \t x \t 1/m");

            foreach (Edge e in edges)
            {
                Console.WriteLine(e.ymin + "\t" + e.ymax + "\t" + e.x + "\t" + e.m_1);
            }



            List<Edge> ordered = edges.OrderBy(min => min.ymin).ThenBy(max => max.ymax).ThenBy(p => p.x).ToList();
            ordered.RemoveAll(a => a.m_1 == Int32.MaxValue);


            Console.WriteLine("\n\nAfter Sorting:");
            Console.WriteLine("ymin \t ymax \t x \t 1/m");
            foreach (Edge e in ordered)
            {
                Console.WriteLine(e.ymin + "\t" + e.ymax + "\t" + e.x + "\t" + e.m_1);
            }

            Console.WriteLine("\n\nAgain without:");
            Console.WriteLine("ymin \t ymax \t x \t 1/m");
            foreach (Edge e in edges)
            {
                Console.WriteLine(e.ymin + "\t" + e.ymax + "\t" + e.x + "\t" + e.m_1);
            }




            int xx = 1;
            Assert.AreEqual(1, xx);
        }

       


    }
}