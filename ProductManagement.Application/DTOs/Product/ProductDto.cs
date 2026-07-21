namespace ProductManagement.Application.DTOs.Product;

public class ProductDto
{
    public int Id { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }
}