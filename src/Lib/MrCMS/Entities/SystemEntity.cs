﻿using System;
using System.ComponentModel;
using MrCMS.DbConfiguration;
using MrCMS.Entities.People;

namespace MrCMS.Entities
{
    public abstract class SystemEntity
    {
        private Guid _guid;

        protected SystemEntity()
        {
            _guid = Guid.NewGuid();
        }

        public virtual int Id { get; set; }

        [DbNotNullable, ShouldMap]
        public virtual Guid Guid => _guid;

        [DisplayName("Created On")]
        public virtual DateTime CreatedOn { get; set; }

        [DisplayName("Updated On")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Only to be used for data import and the like. This is otherwise generated by the system.
        /// </summary>
        /// <param name="guid"></param>
        public virtual void SetGuid(Guid guid)
        {
            _guid = guid;
        }
        
        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }
    }
}