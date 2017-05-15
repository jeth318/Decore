namespace StudentServiceApplication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Students
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string UnionName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? UnionExpiration { get; set; }

        [StringLength(50)]
        public string ProgramCode { get; set; }
    }
}
