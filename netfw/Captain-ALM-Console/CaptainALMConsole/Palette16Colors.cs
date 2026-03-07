using System;
using System.Drawing;

namespace captainalm.calmcmd.console
{
    sealed class Palette16Colors
    {
        private object slockp = new object();
        private Color[] palette = new Color[16];

        public Palette16Colors() { }
        public Palette16Colors(Color[] colIn)
        {
            palette = colIn;
        }

        public static double ColorDistance(Color col1, Color col2)
        {
            double rmean = ((double)col1.R + (double)col2.R) / 2.0D;
            double r = (double)col1.R - (double)col2.R;
            double g = (double)col1.G - (double)col2.G;
            double b = (double)col1.B - (double)col2.B;
            return Math.Sqrt(4 * Math.Pow(g, 2) + ((rmean < 128) ? 2 : 3) * Math.Pow(r, 2) + ((rmean < 128) ? 3 : 2) * Math.Pow(b, 2));
        }

        public Int32 ClosestColor(Color col)
        {
            var idx = -1;
            lock (slockp)
            {
                var smol = double.MaxValue;
                for (int i = 0; i < 16; i++)
                {
                    var c = ColorDistance(col, palette[i]);
                    if (c < smol)
                    {
                        smol = c;
                        idx = i;
                    }
                }
            }
            return idx;
        }

        public Color this[int index]
        {
            get { lock (slockp) return palette[index]; }
            set
            {
                lock (slockp)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (value.ToArgb() == palette[i].ToArgb()) return;
                    }
                    palette[index] = value;
                }
            }
        }
    }
}
