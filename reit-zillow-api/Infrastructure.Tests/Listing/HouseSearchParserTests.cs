using Core.Listing;
using Infrastructure.Listing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Listing
{
    public class HouseSearchParserTests
    {
        private readonly IHouseSearchParser _houseSearchParser;

        public HouseSearchParserTests()
        {
            _houseSearchParser = new HouseSearchParser();
        }

        [Fact]
        public void ParseListingAddresses()
        {
            // arrange 
            string html = File.ReadAllText("TestFiles"
                + Path.DirectorySeparatorChar +
                "zillow_search_house_by_zip.html");
            // act
            IList<string> addresses = _houseSearchParser.ParseListingAddresses(html);
            // assert
            IList<string> expectedAddresses = new List<string>()
            {
                "205 W Camden Ave, Anaheim, CA 92805",
                "1054 S Verde St, Anaheim, CA 92805",
                "1473 E Rosewood Ave, Anaheim, CA 92805",
                "938 N Harbor Blvd, Anaheim, CA 92805",
                "1080 N Baxter St, Anaheim, CA 92805",
                "1424 E Bassett Way, Anaheim, CA 92805",
                "568 S Wayside St, Anaheim, CA 92805",
                "130 W Valencia Ave, Anaheim, CA 92805",
                "1211 E Wilhelmina St, Anaheim, CA 92805",
                "722 S Citron St, Anaheim, CA 92805",
                "981 S Sarah Way, Anaheim, CA 92805",
                "212 E Charlotte Ave, Anaheim, CA 92805",
                "210 S Vine St, Anaheim, CA 92805",
                "1213 E Adele St, Anaheim, CA 92805",
                "703 N Janss St, Anaheim, CA 92805",
                "597 E Chartres St, Anaheim, CA 92805",
                "314 S Illinois St, Anaheim, CA 92805",
                "626 S Harbor Blvd, Anaheim, CA 92805",
                "1733 E Belmont Ave, Anaheim, CA 92805",
                "211 E Wilhelmina St, Anaheim, CA 92805",
                "613 N Glenwood Pl, Anaheim, CA 92805",
                "1727 E Chelsea Dr, Anaheim, CA 92805",
                "1150 N Brantford St, Anaheim, CA 92805"
            };
            Assert.NotNull(addresses);
            Assert.Equal(expectedAddresses.Count, addresses.Count);
            Assert.False(addresses.Except(expectedAddresses).Any());

        }
    }
}
