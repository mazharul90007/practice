namespace Services;

public class CitiesService
{
    private List<string> _cities = new List<string>();
    public CitiesService()
    {
        _cities = new List<string>()
        {
            "London",
            "Paris",
            "New York",
            "Rome"
        };
    }

    //Get Cities
    public List<string> GetCities()
    {
        return _cities;
    }

}
