




using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetManagerMvc.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }
        public string UserPrincipalName { get; set; }
        [Display(Name = "Surname")]
        public string Sn { get; set; }        
        [Display(Name = "Email")]
        public string Mail { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }
    }
    public class Asset
    {
        public int AssetId { get; set; }
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }
        [Display(Name = "Purchase Date")]        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PurchaseDate { get; set; }
        [Display(Name = "Price")]
        public decimal? PurchasePrice { get; set; }
        public string Owner { get; set; }
        // public List<UsePeriod> UserPeriods { get; set; }
        public string Supplier { get; set; }
        public string Manufacturer { get; set; }

        public string CompoundId
        {
            get
            {
                string compoundid = AssetId.ToString();
                if (this is Computer) compoundid = "C" + compoundid;
                return compoundid;
            }
        }

        private readonly ObservableListSource<UsePeriod> _usePeriods =
                new ObservableListSource<UsePeriod>();

        public virtual ObservableListSource<UsePeriod> UsePeriods { get { return _usePeriods; } }
    }
    public class Computer : Asset
    {
        [Display(Name = "Computer Name")]
        public string ComputerName { get; set; }
        [Display(Name = "Computer Type")]
        public string ComputerType { get; set; }
        [Display(Name = "Office Version")]
        public string OfficeVersion { get; set; }
        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; }
        public string Browser { get; set; }
        public string AntiVirus { get; set; }
        [Display(Name = "TeamViewer")]
        public bool IsTeamViewerInstalled { get; set; }
    }
    public class UsePeriod
    {
        public int UsePeriodId { get; set; }
        public int? UserAccountId { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        // TODO check overlap
        // bool overlap = a.start < b.end && b.start < a.end;
        // test excel tosql
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }
        public string Remark { get; set; }
        public string Function { get; set; }

        public int? UsePeriodStatusId { get; set; }
        public virtual UsePeriodStatus Status { get; set; }

        public int AssetId { get; set; }
        public virtual Asset Asset { get; set; }
    }
    public class UsePeriodStatus
    {
        public int UsePeriodStatusId { get; set; }
        public string Description { get; set; }
        public int ColorCode { get; set; }
        public UsePeriodStatus cbField { get { return this; } }
    }
    public class Incident
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Asset Asset { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
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

    }
    public class ObservableListSource<T> : ObservableCollection<T>, IListSource
        where T : class
    {
        private IBindingList _bindingList;

        bool IListSource.ContainsListCollection { get { return false; } }

        IList IListSource.GetList()
        {
            return _bindingList ?? (_bindingList = this.ToBindingList());
        }
    }
}
