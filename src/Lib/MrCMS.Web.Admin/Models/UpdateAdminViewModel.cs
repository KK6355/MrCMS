using System.Collections.Generic;
using MrCMS.Entities;
using MrCMS.Web.Admin.Infrastructure.Models;

namespace MrCMS.Web.Admin.Models
{
    public abstract class UpdateAdminViewModel<T> : IUpdateAdminViewModel where T : SystemEntity
    {
        public int Id { get; set; }
        public ICollection<object> Models { get; set; }
    }
}