using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("TBL_COUNTRY")]
    public class Country
    {
        #region Initial
        public Country()
        {
            Users = new HashSet<User>();
        }
        #endregion

        #region Primary key
        [Key]
        [Column("CNY_CD")]
        [StringLength(128)]
        public string CountryId { get; set; }
        #endregion

        #region Properties
        [Column("REGION")]
        [StringLength(2048)]
        public string Region { get; set; }

        [Required]
        [Column("CNY_NA")]
        [StringLength(2048)]
        public string CountryName { get; set; }

        [Required]
        [StringLength(128)]
        [Column("CCY_SIG")]
        public string CurrencySig { get; set; }

        [Required]
        [StringLength(2048)]
        [Column("CCY_NA")]
        public string CurrencyName { get; set; }
        #endregion

        #region Relationship
        public ICollection<User> Users { get; set; }
        #endregion
    }
}