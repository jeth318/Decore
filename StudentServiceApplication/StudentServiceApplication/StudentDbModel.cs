namespace StudentServiceApplication
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudentDbModel : DbContext
    {
        public StudentDbModel()
            : base("name=StudentDb")
        {
        }

        public virtual DbSet<Students> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>()
                .Property(e => e.UnionName)
                .IsUnicode(false);

            modelBuilder.Entity<Students>()
                .Property(e => e.ProgramCode)
                .IsUnicode(false);
        }
    }
}
