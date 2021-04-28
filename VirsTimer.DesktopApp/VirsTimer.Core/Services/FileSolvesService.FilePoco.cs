using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public partial class FileSolvesService
    {
        private class FilePoco
        {
            
        }

        public class SolvesCollection : KeyedCollection<Guid, Solve>
        {
            protected override Guid GetKeyForItem(Solve item)
            {
                return item.Id;
            }
        }
    }
}
