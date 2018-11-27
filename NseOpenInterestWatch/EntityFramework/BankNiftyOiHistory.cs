namespace NseOpenInterestWatch.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BankNiftyOiHistory")]
    public partial class BankNiftyOiHistory
    {
        public int Id { get; set; }

        public int? StrikePrice { get; set; }

        public int? CeOi { get; set; }

        public int? CeChange { get; set; }

        public int? PeOi { get; set; }

        public int? PeChange { get; set; }

        public DateTime? ModifiedDateTime { get; set; }
    }
}
