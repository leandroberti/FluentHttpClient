using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LMB.FluentHttpClient.Test
{
    [TestClass]
    public class FluentHttpClientTest
    {
        internal class Category
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        internal class Tag
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
        internal class Pet
        {
            public long Id { get; set; }
            public Category Category { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public List<string> PhotoUrls { get; set; }
            public List<Tag> Tags { get; set; }
        }

        [TestMethod]
        public void CallPetService()
        {
            // Arrange

            // Act
            var petList = FluentHttpClient.FromServiceApi("http://petstore.swagger.io", "v2/pet/findByStatus?status=available")
                .WithJsonContent()
                .Get<ICollection<Pet>>();

            // Assert
            Assert.IsTrue(petList != null &&
                petList.IsSuccessStatusCode &&
                petList.ResponseBody.Count > 0);
        }
    }
}
