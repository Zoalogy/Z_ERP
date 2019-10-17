namespace Z_ERP.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MainModel : DbContext
    {
        public MainModel()
            : base("name=MainModel")
        {
        }

        public virtual DbSet<co_Branches> co_Branches { get; set; }
        public virtual DbSet<co_Company> co_Company { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<inv_Stror_to_Store_order> inv_Stror_to_Store_order { get; set; }
        public virtual DbSet<pur_GoodSExchange_Store_StoreHisory> pur_GoodSExchange_Store_StoreHisory { get; set; }


        public virtual DbSet<hr_DedicationAllowanceList> hr_DedicationAllowanceList { get; set; }
        public virtual DbSet<hr_DedicationOrAllowance> hr_DedicationOrAllowance { get; set; }
        public virtual DbSet<hr_Department> hr_Department { get; set; }
        public virtual DbSet<hr_EmpDedicationAllowance> hr_EmpDedicationAllowance { get; set; }
        public virtual DbSet<hr_EmployeeDebtRecords> hr_EmployeeDebtRecords { get; set; }
        public virtual DbSet<hr_Employees> hr_Employees { get; set; }
        public virtual DbSet<hr_EmployeeSalary> hr_EmployeeSalary { get; set; }
        public virtual DbSet<hr_EmployeeSatus> hr_EmployeeSatus { get; set; }
        public virtual DbSet<hr_Expenses> hr_Expenses { get; set; }
        public virtual DbSet<hr_Gender> hr_Gender { get; set; }
        public virtual DbSet<hr_JobsName> hr_JobsName { get; set; }
        public virtual DbSet<hr_SalaryHistory> hr_SalaryHistory { get; set; }
        public virtual DbSet<inv_Brand> inv_Brand { get; set; }
        public virtual DbSet<inv_Categories> inv_Categories { get; set; }
        public virtual DbSet<inv_Inventory> inv_Inventory { get; set; }
        public virtual DbSet<inv_ItemHistory> inv_ItemHistory { get; set; }
        public virtual DbSet<inv_ItemHistoryProccessTypes> inv_ItemHistoryProccessTypes { get; set; }
        public virtual DbSet<inv_Items> inv_Items { get; set; }
        public virtual DbSet<inv_RequestCart> inv_RequestCart { get; set; }
        public virtual DbSet<inv_RequestItems> inv_RequestItems { get; set; }
        public virtual DbSet<inv_Requests> inv_Requests { get; set; }
        public virtual DbSet<inv_SubCategories> inv_SubCategories { get; set; }
        public virtual DbSet<pay_CustomerAcounts> pay_CustomerAcounts { get; set; }
        public virtual DbSet<pay_CustomerBankAcounts> pay_CustomerBankAcounts { get; set; }
        public virtual DbSet<pay_Installments> pay_Installments { get; set; }
        public virtual DbSet<pay_Payment> pay_Payment { get; set; }
        public virtual DbSet<pay_PaymentMethod> pay_PaymentMethod { get; set; }
        public virtual DbSet<pay_SuplierAcounts> pay_SuplierAcounts { get; set; }
        public virtual DbSet<pur_BillDetails> pur_BillDetails { get; set; }
        public virtual DbSet<pur_Bills> pur_Bills { get; set; }
        public virtual DbSet<pur_Purchase> pur_Purchase { get; set; }
        public virtual DbSet<pur_PurchaseCart> pur_PurchaseCart { get; set; }
        public virtual DbSet<pur_Supliers> pur_Supliers { get; set; }
        public virtual DbSet<RequestSourceType> RequestSourceTypes { get; set; }
        public virtual DbSet<sal_Customer> sal_Customer { get; set; }
        public virtual DbSet<sal_pointOfSale> sal_pointOfSale { get; set; }
        public virtual DbSet<sal_Reciept> sal_Reciept { get; set; }
        public virtual DbSet<sal_RecieptDetails> sal_RecieptDetails { get; set; }
        public virtual DbSet<sal_SaleItems> sal_SaleItems { get; set; }
        public virtual DbSet<sal_Sales> sal_Sales { get; set; }
        public virtual DbSet<sal_SalesCart> sal_SalesCart { get; set; }
        public virtual DbSet<sal_SalesItemHistory> sal_SalesItemHistory { get; set; }
        public virtual DbSet<sys_Branches> sys_Branches { get; set; }
        public virtual DbSet<sys_PermissionLabels> sys_PermissionLabels { get; set; }
        public virtual DbSet<sys_SystemPermissions> sys_SystemPermissions { get; set; }
        public virtual DbSet<sys_SystemScreenDialogs> sys_SystemScreenDialogs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<trc_Expenses> trc_Expenses { get; set; }
        public virtual DbSet<trc_Trips> trc_Trips { get; set; }
        public virtual DbSet<trc_TripStatus> trc_TripStatus { get; set; }
        public virtual DbSet<trc_Trucks> trc_Trucks { get; set; }
        public virtual DbSet<sys_Clients> sys_Clients { get; set; }
        public virtual DbSet<sys_Dashboard> sys_Dashboard { get; set; }
        public virtual DbSet<sys_Notification> sys_Notification { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<hr_EmpDedicationAllowance>()
                .Property(e => e.EmpBatchValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<hr_EmployeeDebtRecords>()
                .Property(e => e.DebtRecordsAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<hr_JobsName>()
                .Property(e => e.JobNameBasicSalary)
                .HasPrecision(18, 0);

            modelBuilder.Entity<inv_Inventory>()
                .Property(e => e.InvertoryRent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<inv_RequestCart>()
                .Property(e => e.ItemPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<pay_Installments>()
                .Property(e => e.RecieptNo)
                .IsFixedLength();

            modelBuilder.Entity<pur_Bills>()
                .Property(e => e.BillTaxes)
                .IsFixedLength();

            modelBuilder.Entity<pur_PurchaseCart>()
                .Property(e => e.ItempurchasePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<pur_PurchaseCart>()
                .Property(e => e.ItemSellPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_Sales>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_Sales>()
                .Property(e => e.ItemSalePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_Sales>()
                .Property(e => e.ItemPurchasePrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_Sales>()
                .Property(e => e.ItemTotalSaleAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sal_SalesCart>()
                .Property(e => e.ItemPrice)
                .HasPrecision(18, 0);
             
            modelBuilder.Entity<sys_PermissionLabels>()
                .Property(e => e.ComponentTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<sys_PermissionLabels>()
                .Property(e => e.ComponentKeyName)
                .IsUnicode(false);

            modelBuilder.Entity<sys_SystemPermissions>()
                .Property(e => e.ScreenDailogID)
                .IsUnicode(false);

            modelBuilder.Entity<sys_SystemScreenDialogs>()
                .Property(e => e.ScreenDailogID)
                .IsUnicode(false);

            modelBuilder.Entity<sys_SystemScreenDialogs>()
                .Property(e => e.FontIcon)
                .IsUnicode(false);

            modelBuilder.Entity<sys_SystemScreenDialogs>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<sys_Dashboard>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<sys_Notification>()
                .Property(e => e.Url)
                .IsUnicode(false);
        }

        
    }
}
