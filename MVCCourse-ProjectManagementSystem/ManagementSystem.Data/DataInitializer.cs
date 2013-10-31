namespace ManagementSystem.Data
{
    using System.Data.Entity;

    public class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            base.Seed(context);
        }
    }
}
