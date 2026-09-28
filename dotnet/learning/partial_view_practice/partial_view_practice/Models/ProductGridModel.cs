namespace Models
{
    public class ProductGrid
    {
        public string GridTitle {get; set;} = string.Empty;
        public List<Product> products {get; set;} = new List<Product>();

    }
}