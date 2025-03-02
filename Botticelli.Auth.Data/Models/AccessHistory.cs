using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Botticelli.Auth.Data.Models;

/// <summary>
///     Access history item
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class AccessHistory<TEntity>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /// <summary>
    ///     Access item entity (login/user info)
    /// </summary>
    public TEntity? Entity { get; set; }

    /// <summary>
    ///     Timestamp
    /// </summary>
    public required DateTime TimestampUtc { get; set; }

    public bool IsSuccess { get; set; }

    [MaxLength(2048)]
    public string? ErrorMessage { get; set; }
}