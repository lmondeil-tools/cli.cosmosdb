using Microsoft.VisualStudio.TestTools.UnitTesting;
using lmondeil.cli.cosmosdb.services.Helpers;
using lmondeil.cli.cosmosdb.services.Models;
using Microsoft.Azure.Cosmos;
using FluentAssertions;

namespace lmondeil.cli.cosmosdb.services.Helpers.tests
{
    [TestClass()]
    public class PatchOperationHelperTests
    {
        static IEnumerable<object[]> BuildFromTestDataSource()
        {
            // Set basic with string value
            yield return new object[] { new PatchEntity(PatchType.Set, "/name", "Pierre", "System.String"), PatchOperation.Set("/name", "Pierre"), typeof(string) };

            // Set basic with int value
            yield return new object[] { new PatchEntity(PatchType.Set, "/age", "12", "System.Int32"), PatchOperation.Set("/age", 12), typeof(int) };

            // Set basic with string[] value
            yield return new object[] { new PatchEntity(PatchType.Set, "/hobbies", "['surf', 'skate']", "System.String[]"), PatchOperation.Set("/hobbies", new string[] { "surf", "skate" }), typeof(string[]) };

            // Set having no "/" at the begining of the path
            yield return new object[] { new PatchEntity(PatchType.Set, "name", "Pierre", "System.String"), PatchOperation.Set("/name", "Pierre"), typeof(string) };
        }

        [TestMethod()]
        [DynamicData(nameof(BuildFromTestDataSource), DynamicDataSourceType.Method)]
        public void BuildFromTest(PatchEntity input, PatchOperation expectation, Type expectationValueType)
        {
            // Act
            var result = PatchOperationHelper.BuildFrom(input);

            // Assert
            result.Should().BeEquivalentTo(expectation);
            var value = result.GetType().GetProperty("Value")!.GetValue(result);
            value.Should().BeOfType(expectationValueType);
        }
    }
}