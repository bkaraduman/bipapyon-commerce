using BiPapyon.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPapyon.Api.Domain.Models
{
    public class ProductVote:BaseEntity
    {
        public Guid ProductId { get; set; }
        public VoteType VoteType { get; set; }
        public Guid CreatedById { get; set; }
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
