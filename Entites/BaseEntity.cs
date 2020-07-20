using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Educati.Azure.Function.Api.Entites
{
    public abstract class BaseEntity: TableEntity
    {
        public Guid Id { get; set; }
        public bool? Active { set; get; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
