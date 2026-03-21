using Xunit;
using EventsApi.Repos;
using EventsApi.Models;
namespace EventsApi.Tests
{
    public class RepoTest
    {
        [Fact]
        public void RepoList_WithSeedData_IsNotEmpty()
        {
            // Arrange & Act
            var repo = new RepoList<Event>(includeData: true);
            var events = repo.GetAll();

            // Assert
            Assert.NotEmpty(events);
        }

        [Fact]
        public void RepoList_FilterByCategory_ReturnsOnlyMatchingEvents()
        {
            // Arrange
            var repo = new RepoList<Event>(includeData: true);

            /// Act
            var result = repo.GetAll(category: "Music").ToList();

            /// Assert
            Assert.NotEmpty(result);
            Assert.All(result, e =>
                Assert.Contains("Music", e.Category, StringComparison.OrdinalIgnoreCase));
        }
    }
}
