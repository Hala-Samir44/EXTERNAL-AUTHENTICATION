using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.model
{
    public class RolePermissions
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Role")]
        public int roleId { get; set; }
        [ForeignKey("Permission")]
        public int permissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
