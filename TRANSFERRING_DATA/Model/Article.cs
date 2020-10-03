namespace Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Article")]
    public partial class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? Number { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? Time { get; set; }

        public bool? Status { get; set; }
    }
}
