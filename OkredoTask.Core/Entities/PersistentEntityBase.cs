using System;

namespace OkredoTask.Core.Entities
{
    public abstract class PersistentEntityBase
    {
        public virtual DateTime CreatedOn { get; protected set; }
        public virtual DateTime? ModifiedOn { get; protected set; }
        public virtual DateTime? DeletedOn { get; protected set; }
    }
}