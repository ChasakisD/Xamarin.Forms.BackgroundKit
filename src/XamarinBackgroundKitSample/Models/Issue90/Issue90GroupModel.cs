using System.Collections.Generic;

namespace XamarinBackgroundKitSample.Models.Issue90
{
    internal class Issue90GroupModel : List<Issue90ItemModel>
    {
        public string Key { get; }

        public Issue90GroupModel(string key)
        {
            Key = key;
        }
    }
}
