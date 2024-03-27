﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Model.Models;

public partial class TipoPeso
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string Descripcion { get; set; }

    public bool? Eliminado { get; set; }

    [InverseProperty("TipoPesoNavigation")]
    public virtual ICollection<BitacoraPeso> BitacoraPeso { get; set; } = new List<BitacoraPeso>();

    [InverseProperty("TipoPesoNavigation")]
    public virtual ICollection<MetaSalud> MetaSalud { get; set; } = new List<MetaSalud>();

    [InverseProperty("TipoPesoNavigation")]
    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}