﻿using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public abstract class NamedEntity : BaseEntity, INamedEntity
    {
        /// <summary> Имя </summary>
        [Required]
        public string Name { get; set; }
    }
}
