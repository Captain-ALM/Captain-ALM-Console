using System;

namespace captainalm.calmcmd
{
    /// <summary>
    /// Wraps a name store with an <see cref="captainalm.calmcmd.IName">IName</see> wrapper.
    /// </summary>
    public struct Name : IName
    {
        private string _name;
        private string _owner;

        /// <summary>
        /// Constructs a new name store with the specified owner and name
        /// </summary>
        /// <param name="ownerIn">The owner</param>
        /// <param name="nameIn">The name</param>
        /// <exception cref="System.ArgumentNullException">a provided parameter is null</exception>
        public Name(string ownerIn, string nameIn)
            : this()
        {
            this.owner = ownerIn;
            this.name = nameIn;
        }

        /// <summary>
        /// Returns the name of the class
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the provided parameter is null</exception>
        public string name
        {
            get { return (_name == null) ? "" : _name; }
            set { if (object.ReferenceEquals(value, null)) { throw new ArgumentNullException("value"); } _name = value; }
        }
        /// <summary>
        /// Returns the owner of the class
        /// </summary>
        /// <exception cref="System.ArgumentNullException">the provided parameter is null</exception>
        public string owner
        {
            get { return (_owner == null) ? "" : _owner; }
            set { if (object.ReferenceEquals(value, null)) { throw new ArgumentNullException("value"); } _owner = value; }
        }

        /// <summary>
        /// Returns the name in the form owner.name of the object
        /// (Or just name if owner is "")
        /// </summary>
        /// <returns>The owner.name form of the object</returns>
        public override string ToString()
        {
            return (_owner == null || _owner.Equals("")) ? ((_name == null || _name.Equals("")) ? base.ToString() : _name) : _owner + "." + _name;
        }
    }
}
