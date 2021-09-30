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
            long rmean = ((long)col1.R + (long)col2.R) / 2;
            long r = (long)col1.R - (long)col2.R;
            long g = (long)col1.G - (long)col2.G;
            long b = (long)col1.B - (long)col2.B;
            return Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
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
