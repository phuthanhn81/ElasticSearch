namespace Model
{
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Text(Name = "id")]
        public int ID { get; set; }

        public DateTime? Birth { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
