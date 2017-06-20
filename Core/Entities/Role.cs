using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Role
    {
        #region Scalar Properties
        public int RoleId { get; set; }
        public string Name { get; set; }
        #endregion

        #region Navigation Properties
        public ICollection<User> Users { get; set; } = new List<User>();

        #endregion
    }
}