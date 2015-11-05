using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AssetManagerMvc.Models
{
    public class AssetManagerContext : DbContext
    {
        public AssetManagerContext() : base()
        {

        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UsePeriod> UsePeriods { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<UsePeriodStatus> UsePeriodStatuses { get; set; }
        public DbSet<Printer> Printers { get; set; }

    }
    //public class ObservableListSource<T> : ObservableCollection<T>, IListSource
    //    where T : class
    //{
    //    private IBindingList _bindingList;

    //    bool IListSource.ContainsListCollection { get { return false; } }

    //    IList IListSource.GetList()
    //    {
    //        return _bindingList ?? (_bindingList = this.ToBindingList());
    //    }
    //}
}