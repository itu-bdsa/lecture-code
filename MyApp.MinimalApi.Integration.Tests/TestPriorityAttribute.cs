namespace MyApp.MinimalApi.Integration.Tests;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TestPriorityAttribute : Attribute
{
    public int Priority { get; init; }

    public TestPriorityAttribute(int priority) => Priority = priority;
}
