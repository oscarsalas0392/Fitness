﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Fitness.Model.Models;

public partial class Dieta
{
    public int Id { get; set; }

    public int Usuario { get; set; }

    public DateTime? Fecha { get; set; }

    public int TipoComida { get; set; }

    public int Calorias { get; set; }

    public string Comentarios { get; set; }

    public bool? Eliminado { get; set; }

    public virtual ICollection<AlimentoConsumido> AlimentoConsumido { get; set; } = new List<AlimentoConsumido>();

    public virtual TipoComida TipoComidaNavigation { get; set; }

    public virtual Usuario UsuarioNavigation { get; set; }
}