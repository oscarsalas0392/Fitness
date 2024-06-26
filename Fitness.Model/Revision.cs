﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Fitness.Model.Models;

public partial class Revision
{
    public int Id { get; set; }

    public int Usuario { get; set; }

    public DateTime Fecha { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<ActividadFisica> ActividadFisicas { get; set; } = new List<ActividadFisica>();

    public virtual ICollection<Dieta> Dieta { get; set; } = new List<Dieta>();

    public virtual ICollection<MetaSalud> MetaSaluds { get; set; } = new List<MetaSalud>();

    public virtual Usuario UsuarioNavigation { get; set; }
}