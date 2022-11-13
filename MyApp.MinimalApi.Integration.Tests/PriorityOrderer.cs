using Xunit.Abstractions;
using Xunit.Sdk;

namespace MyApp.MinimalApi.Integration.Tests
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var assembly = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
            var methods = new SortedDictionary<int, List<TTestCase>>();

            foreach (TTestCase testCase in testCases)
            {
                var priority = testCase.TestMethod.Method
                    .GetCustomAttributes(assembly)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                GetOrCreate(methods, priority).Add(testCase);
            }

            foreach (TTestCase testCase in
                methods.Keys.SelectMany(
                    priority => methods[priority].OrderBy(
                        testCase => testCase.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue? result)
                ? result
                : (dictionary[key] = new TValue());
    }
}
