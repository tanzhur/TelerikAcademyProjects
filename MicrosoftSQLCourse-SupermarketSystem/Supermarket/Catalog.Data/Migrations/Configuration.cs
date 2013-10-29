namespace Catalog.Data.Migrations
{
    using MySQLCatalog.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DropCreateDatabaseAlways<Catalog.Data.CatalogContext>
    {
        //public Configuration()
        //{
        //    AutomaticMigrationsEnabled = true;
        //    this.AutomaticMigrationDataLossAllowed = true;
        //}

        protected override void Seed(Catalog.Data.CatalogContext msDB)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            //msDB.Database.Connection.
            //msDB.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Vendors OFF");

            using (var db = new MySQLCatalogEntities())
            {

                foreach (var product in db.Products)
                {
                    var newProduct = new Product
                    {
                        MeasureID = product.MeasureID,
                        BasePrice = product.BasePrice,
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        VendorID = product.VendorID

                    };

                    //Console.WriteLine(product.ProductID);

                    msDB.Products.AddOrUpdate(newProduct);

                }

                foreach (var measure in db.Measures)
                {
                    var newMeasure = new Measure
                    {
                        MeasureID = measure.MeasureID,
                        MeasureName = measure.MeasureName
                    };

                    msDB.Measures.AddOrUpdate(newMeasure);
                }

                foreach (var vendor in db.Vendors)
                {
                    var newVendor = new Vendor
                    {
                        Name = vendor.Name,
                        VendorId = vendor.VendorId

                    };
                    msDB.Vendors.AddOrUpdate(newVendor);
                }

                // msDB.SaveChanges();
            }
        }
    }
}