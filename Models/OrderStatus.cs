using System.ComponentModel.DataAnnotations;

public class OrderStatus
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Status Name")]
    public string? StatusName { get; set; }
}