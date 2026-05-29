namespace RestaurantManager.Domain.Common;

public abstract class AuditBase
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true; // Borrado lógico (no elimina de la BD)
}