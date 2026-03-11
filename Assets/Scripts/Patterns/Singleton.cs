public class Singleton<T> where T : new()
{
    /// <summary>
    /// Private static field to hold the single instance of the class
    /// </summary>
    private static T _instance;

    /// <summary>
    ///  Private constructor to prevent instantiation from outside the class
    /// </summary>
    private Singleton() { }


    /// <summary>
    /// Public property to get the instance of the class
    /// </summary> 
    public static T Instance
    {
        get
        {
            // If the instance is not created yet, create it
            if (_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }
}
