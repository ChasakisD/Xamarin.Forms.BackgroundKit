using System.Collections.Generic;

namespace XamarinBackgroundKitSample.Models.Issue90
{
    internal class Issue90ViewModel
    {
        public List<Issue90GroupModel> GroupItemList { get; }

        public Issue90ViewModel()
        {
            var group1 = new Issue90GroupModel("Group1")
            {
                new Issue90ItemModel("Item1", true, true)
            };
            var group2 = new Issue90GroupModel("Group2")
            {
                new Issue90ItemModel("Item2", true, false),
                new Issue90ItemModel("Item3", false, false),
                new Issue90ItemModel("Item4", false, false),
                new Issue90ItemModel("Item5", false, true)
            };
            var group3 = new Issue90GroupModel("Group3")
            {
                new Issue90ItemModel("Item6", true, false),
                new Issue90ItemModel("Item7", false, true)
            };
            var group4 = new Issue90GroupModel("Group4")
            {
                new Issue90ItemModel("Item8", true, false),
                new Issue90ItemModel("Item9", false, false),
                new Issue90ItemModel("Item10", false, false),
                new Issue90ItemModel("Item11", false, true)
            };
            var group5 = new Issue90GroupModel("Group5")
            {
                new Issue90ItemModel("Item12", true, true)
            };

            GroupItemList = new List<Issue90GroupModel>
            {
                group1,
                group2,
                group3,
                group4,
                group5
            };
        }
    }
}
