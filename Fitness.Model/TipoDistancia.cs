﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Fitness.Model.Models;

public partial class TipoDistancia
{
    public int Id { get; set; }

    public string Descripcion { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<ActividadFisica> ActividadFisicas { get; set; } = new List<ActividadFisica>();
}