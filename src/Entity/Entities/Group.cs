using Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("TBL_GROUP")]
    public class Group : FullAuditedEntity
    {
        #region Initial
        public Group()
        {

        }
        #endregion

        #region Primary key
        [Column("GROUP_CD")]
        [StringLength(128)]
        public string GroupCode { get; set; }
        #endregion

        #region Properties
        [Column("GROUP_NA")]
        [StringLength(2048)]
        public string GroupName { get; set; }
        #endregion
    }
}