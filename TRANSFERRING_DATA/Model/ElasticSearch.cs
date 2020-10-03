namespace Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ElasticSearch : DbContext
    {
        public ElasticSearch()
            : base("name=ElasticSearch")
        {
        }

        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
