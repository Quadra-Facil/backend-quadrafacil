﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuadraFacil_backend.Models.Arena.Space;

public class SpaceModel
{
    [Key]
    public int SpaceId { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(150)]
    public string? Name { get; set; }

    [JsonIgnore]
    public string? Status { get; set; }

    //referencia a arena
    [ForeignKey("Arena")]
    public int ArenaId { get; set; }

    //[JsonIgnore]
    public ArenaModel? Arena { get; set; }

}