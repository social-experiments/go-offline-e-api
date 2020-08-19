using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace goOfflineE.Entites
{
    public abstract class BaseEntity : TableEntity
    {
        public bool? Active { set; get; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
