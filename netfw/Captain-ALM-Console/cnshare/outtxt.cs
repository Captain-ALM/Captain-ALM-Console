using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace captainalm.calmcon.api
{
    /// <summary>
    /// This class is used to output text to the rich textbox output.
    /// </summary>
    /// <remarks></remarks>
    public class OutputText
    {
        private List<TextBlock> _blocks = new List<TextBlock>();
        /// <summary>
        /// Creates a new output text instance
        /// </summary>
        /// <param name="txt">The intal text.</param>
        /// <param name="forecol">The intal forecolor.</param>
        /// <param name="bld">Is the intal text bold.</param>
        /// <param name="itl">Is the intal text italic.</param>
        /// <param name="ul">Is the intal text underlined.</param>
        /// <param name="so">Is the intal text strikeout.</param>
        /// <remarks></remarks>
        public OutputText(string txt = "", System.Drawing.Color forecol = default(System.Drawing.Color), bool bld = false, bool itl = false, bool ul = false, bool so = false)
        {
            if (!Object.ReferenceEquals(txt, null) && txt != "")
            {
                _blocks.Add(new TextBlock(txt, forecol, bld, itl, ul, so));
            }
        }
        //Only accessible in the Shared & API Library.
        internal OutputText(OutputTextBlock[] otba)
        {
            foreach (OutputTextBlock otb in otba)
            {
                _blocks.Add(new TextBlock(otb));
            }
        }
        /// <summary>
        /// Writes text to the current block with the current block's attributes.
        /// </summary>
        /// <param name="txt">The text to write.</param>
        /// <exception cref="InvalidOperationException">Thrown when no blocks to write to.</exception>
        /// <remarks></remarks>
        public void write(string txt)
        {
            if (_blocks.Count != 0)
            {
                _blocks[_blocks.Count - 1].write(txt);
            }
            else
            {
                throw new InvalidOperationException("There are no blocks to write to.");
            }
        }
        /// <summary>
        /// Writes text and a new line to the current block with the current block's attributes.
        /// </summary>
        /// <param name="txt">The text to write.</param>
        /// <exception cref="InvalidOperationException">Thrown when no blocks to write to.</exception>
        /// <remarks></remarks>
        public void writeline(string txt)
        {
            if (_blocks.Count != 0)
            {
                _blocks[_blocks.Count - 1].writeline(txt);
            }
            else
            {
                throw new InvalidOperationException("There are no blocks to write to.");
            }
        }
        /// <summary>
        /// Writes text to a new or the current block depending on the current block's attributes or if there are any blocks at all.
        /// </summary>
        /// <param name="txt">The text.</param>
        /// <param name="forecol">The forecolor.</param>
        /// <param name="bld">Is the text bold.</param>
        /// <param name="itl">Is the text italic.</param>
        /// <param name="ul">Is the text underlined.</param>
        /// <param name="so">Is the text strikeout.</param>
        /// <remarks></remarks>
        public void write(string txt, System.Drawing.Color forecol = default(System.Drawing.Color), bool bld = false, bool itl = false, bool ul = false, bool so = false)
        {
            if (_blocks.Count != 0)
            {
                if (_blocks[_blocks.Count - 1].forecolor == forecol & _blocks[_blocks.Count - 1].bold == bld & _blocks[_blocks.Count - 1].italic == itl & _blocks[_blocks.Count - 1].underline == ul & _blocks[_blocks.Count - 1].strikeout == so)
                {
                    _blocks[_blocks.Count - 1].writeline(txt);
                }
                else
                {
                    _blocks.Add(new TextBlock(txt, forecol, bld, itl, ul, so));
                }
            }
            else
            {
                _blocks.Add(new TextBlock(txt, forecol, bld, itl, ul, so));
            }
        }
        /// <summary>
        /// Writes text and a new line to a new or the current block depending on the current block's attributes or if there are any blocks at all.
        /// </summary>
        /// <param name="txt">The text.</param>
        /// <param name="forecol">The forecolor.</param>
        /// <param name="bld">Is the text bold.</param>
        /// <param name="itl">Is the text italic.</param>
        /// <param name="ul">Is the text underlined.</param>
        /// <param name="so">Is the text strikeout.</param>
        /// <remarks></remarks>
        public void writeline(string txt, System.Drawing.Color forecol = default(System.Drawing.Color), bool bld = false, bool itl = false, bool ul = false, bool so = false)
        {
            if (_blocks.Count != 0)
            {
                if (_blocks[_blocks.Count - 1].forecolor == forecol & _blocks[_blocks.Count - 1].bold == bld & _blocks[_blocks.Count - 1].italic == itl & _blocks[_blocks.Count - 1].underline == ul & _blocks[_blocks.Count - 1].strikeout == so)
                {
                    _blocks[_blocks.Count - 1].writeline(txt);
                }
                else
                {
                    _blocks.Add(new TextBlock(txt + System.Environment.NewLine, forecol, bld, itl, ul, so));
                }
            }
            else
            {
                _blocks.Add(new TextBlock(txt + System.Environment.NewLine, forecol, bld, itl, ul, so));
            }
        }
        /// <summary>
        /// Converts the contents of the OutputText to an array of OutputTextBlock Structures.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public OutputTextBlock[] ToOutputTextBlocks()
        {
            var lst = new List<OutputTextBlock>();
            foreach (TextBlock tb in _blocks)
            {
                lst.Add(tb.ToOutputTextBlock());
            }
            return lst.ToArray();
        }
        /// <summary>
        /// Returns the number of blocks in the OutputText object.
        /// </summary>
        /// <value>Integer.</value>
        /// <returns>The number of blocks in the OutputText object.</returns>
        /// <remarks></remarks>
        public int BlockCount
        {
            get
            {
                return this._blocks.Count;
            }
        }
        /// <summary>
        /// The default property, gets a block from an index.
        /// </summary>
        /// <param name="index">The index number.</param>
        /// <value>OutputTextBlock</value>
        /// <returns>The OutputTextBlock as that index.</returns>
        /// <remarks></remarks>
        public OutputTextBlock this[int index]
        {
            get
            {
                return this._blocks[index].ToOutputTextBlock();
            }
        }
        /// <summary>
        /// Converts a string to outputtext.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The output text instance.</returns>
        /// <remarks></remarks>
        public static implicit operator OutputText(string str)
        {
            return new OutputText(str);
        }
        /// <summary>
        /// Converts an array of outputtextblock to outputtext.
        /// </summary>
        /// <param name="otba">The outputtextblock array to convert.</param>
        /// <returns>The output text instance.</returns>
        /// <remarks></remarks>
        public static implicit operator OutputText(OutputTextBlock[] otba)
        {
            return new OutputText(otba);
        }
        /// <summary>
        /// Converts an outputtext instance to an string.
        /// </summary>
        /// <param name="optxt">The outputtext instance to convert.</param>
        /// <returns>The string held by the outputtext instance.</returns>
        /// <remarks></remarks>
        public static explicit operator string(OutputText optxt)
        {
            var txt = "";
            foreach (TextBlock bk in optxt._blocks)
            {
                txt += bk.text;
            }
            return txt;
        }
        /// <summary>
        /// Converts an outputtext instance to an output text block array.
        /// </summary>
        /// <param name="optxt">The outputtext instance to convert.</param>
        /// <returns>A outputtextblock array from the outputtext instance.</returns>
        /// <remarks></remarks>
        public static explicit operator OutputTextBlock[](OutputText optxt)
        {
            var lst = new List<OutputTextBlock>();
            foreach (TextBlock tb in optxt._blocks)
            {
                lst.Add(tb.ToOutputTextBlock());
            }
            return lst.ToArray();
        }
        /// <summary>
        /// Concats two Output Texts Together.
        /// </summary>
        /// <param name="optxt1">The first output text.</param>
        /// <param name="optxt2">The second output text.</param>
        /// <returns>The concated output text object.</returns>
        /// <remarks></remarks>
        [SpecialName]
        public static OutputText op_Concatenate(OutputText optxt1, OutputText optxt2)
        {
            var noptxt = new OutputText(optxt1.ToOutputTextBlocks());
            foreach (TextBlock cb in optxt2._blocks)
            {
                noptxt.write(cb.text, cb.forecolor, cb.bold, cb.italic, cb.underline, cb.strikeout);
            }
            return noptxt;
        }
        /// <summary>
        /// Checks if two output text strings are not equal.
        /// </summary>
        /// <param name="optxt1">The first output text.</param>
        /// <param name="optxt2">The second output text.</param>
        /// <returns>The boolean of if they are not equal.</returns>
        /// <remarks></remarks>
        public static bool operator !=(OutputText optxt1, OutputText optxt2)
        {
            return !(optxt1 == optxt2);
        }
        /// <summary>
        /// Checks if two output text strings are equal.
        /// </summary>
        /// <param name="optxt1">The first output text.</param>
        /// <param name="optxt2">The second output text.</param>
        /// <returns>The boolean of if they are equal.</returns>
        /// <remarks></remarks>
        public static bool operator ==(OutputText optxt1, OutputText optxt2)
        {
            return optxt1.ToString() == optxt2.ToString();
        }
        /// <summary>
        /// Returns the Value of this Object as a String.
        /// </summary>
        /// <returns>String that this object contains.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            var toret = "";
            foreach (TextBlock b in _blocks)
            {
                toret += b.text;
            }
            return toret;
        }
        //This is the internal textblock instance.
        private class TextBlock
        {
            private string _text = "";
            private System.Drawing.Color _forecolor = System.Drawing.Color.Black;
            private bool _bold = false;
            private bool _italic = false;
            private bool _underline = false;
            private bool _strike = false;

            public TextBlock(OutputTextBlock otb)
            {
                _text = otb.text;
                _forecolor = otb.forecolor;
                _bold = otb.bold;
                _italic = otb.italic;
                _underline = otb.underline;
                _strike = otb.strikeout;
            }

            public TextBlock(string txt = "", System.Drawing.Color forecol = default(System.Drawing.Color), bool bld = false, bool itl = false, bool ul = false, bool so = false)
            {
                _text = txt;
                if (forecol.IsEmpty)
                {
                    _forecolor = System.Drawing.Color.Black;
                }
                else
                {
                    _forecolor = forecol;
                }
                _bold = bld;
                _italic = itl;
                _underline = ul;
                _strike = so;
            }

            public string text
            {
                get
                {
                    return _text;
                }
                set
                {
                    _text = value;
                }
            }

            public System.Drawing.Color forecolor
            {
                get
                {
                    return _forecolor;
                }
                set
                {
                    _forecolor = value;
                }
            }

            public bool bold
            {
                get
                {
                    return _bold;
                }
                set
                {
                    _bold = value;
                }
            }

            public bool italic
            {
                get
                {
                    return _italic;
                }
                set
                {
                    _italic = value;
                }
            }

            public bool underline
            {
                get
                {
                    return _underline;
                }
                set
                {
                    _underline = value;
                }
            }

            public bool strikeout
            {
                get
                {
                    return _strike;
                }
                set
                {
                    _strike = value;
                }
            }

            public void write(string txt)
            {
                _text += txt;
            }

            public void writeline(string txt)
            {
                _text += txt + System.Environment.NewLine;
            }

            public override string ToString()
            {
                return _text;
            }

            public OutputTextBlock ToOutputTextBlock()
            {
                return new OutputTextBlock(_text, _forecolor, _bold, _italic, _underline, _strike);
            }
        }
    }
    /// <summary>
    /// This is the split up parts of an OutputText Instance.
    /// </summary>
    /// <remarks></remarks>
    public struct OutputTextBlock
    {
        /// <summary>
        /// The held text.
        /// </summary>
        /// <remarks></remarks>
        public string text;
        /// <summary>
        /// The foreground colour of the text.
        /// </summary>
        /// <remarks></remarks>
        public System.Drawing.Color forecolor;
        /// <summary>
        /// If the text is bold.
        /// </summary>
        /// <remarks></remarks>
        public bool bold;
        /// <summary>
        /// If the text is italic.
        /// </summary>
        /// <remarks></remarks>
        public bool italic;
        /// <summary>
        /// If the text is underlined.
        /// </summary>
        /// <remarks></remarks>
        public bool underline;
        /// <summary>
        /// If the text is striked out.
        /// </summary>
        /// <remarks></remarks>
        public bool strikeout;
        //Only accessible in the Shared & API Library.
        internal OutputTextBlock(String txt, System.Drawing.Color forecol, bool bld, bool itl, bool ul, bool so)
        {
            text = txt;
            forecolor = forecol;
            bold = bld;
            italic = itl;
            underline = ul;
            strikeout = so;
        }
        /// <summary>
        /// Returns the String held by this object.
        /// </summary>
        /// <returns>The String held by this object.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return text;
        }
    }
}
