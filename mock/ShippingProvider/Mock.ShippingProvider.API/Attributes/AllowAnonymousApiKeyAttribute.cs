namespace Mock.ShippingProvider.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method |  AttributeTargets.Parameter)]
    public class AllowAnonymousApiKeyAttribute : Attribute { }
}
