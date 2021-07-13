using System;
using System.Runtime.CompilerServices;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Provides a string that can be styled for output
    /// </summary>
    public struct StylableString
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
        /// The background colour of the text.
        /// </summary>
        /// <remarks></remarks>
        public System.Drawing.Color backcolor;
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
        /// <summary>
        /// The font to use for the text
        /// <remarks></remarks>
        /// </summary>
        public string fontname;
        /// <summary>
        /// Constructs a new StylableString instance with the specified string
        /// </summary>
        /// <param name="strIn">The string to store</param>
        public StylableString(string strIn) : this()
        {
            text = strIn;
        }
        /// <summary>
        /// Converts a string to StylableString.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The stylable string instance.</returns>
        /// <remarks></remarks>
        public static implicit operator StylableString(string str)
        {
            return new StylableString(str);
        }
        /// <summary>
        /// Converts an StylableString instance to an string.
        /// </summary>
        /// <param name="optxt">The StylableString instance to convert.</param>
        /// <returns>The string held by the StylableString instance.</returns>
        /// <remarks></remarks>
        public static implicit operator string(StylableString optxt)
        {
            return optxt.text;
        }
        /// <summary>
        /// Concats two Stylable Strings Together.
        /// The style would be the first string's style
        /// </summary>
        /// <param name="optxt1">The first Stylable String.</param>
        /// <param name="optxt2">The second Stylable String.</param>
        /// <returns>The concated Stylable String object.</returns>
        /// <remarks></remarks>
        [SpecialName]
        public static StylableString op_Concatenate(StylableString optxt1, StylableString optxt2)
        {
            return optxt1 + optxt2;
        }
        /// <summary>
        /// Concats two Stylable Strings Together.
        /// The style would be the first string's style
        /// </summary>
        /// <param name="optxt1">The first Stylable String.</param>
        /// <param name="optxt2">The second Stylable String.</param>
        /// <returns>The concated Stylable String object.</returns>
        /// <remarks></remarks>
        public static StylableString operator +(StylableString optxt1, StylableString optxt2)
        {
            return new StylableString(optxt1.text + optxt2.text) { backcolor = optxt1.backcolor, forecolor = optxt1.forecolor, bold = optxt1.bold, underline = optxt1.underline, italic = optxt1.italic, strikeout = optxt1.strikeout, fontname = optxt1.fontname };
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
