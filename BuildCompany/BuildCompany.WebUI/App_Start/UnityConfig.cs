using BuildCompany.Core.Contracts;
using BuildCompany.Core.Models;
using System;

using Unity;

namespace BuildCompany.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IRepository<BankAccount>, DataAccess.SQL.SQLRepository<BankAccount>>();
            container.RegisterType<IRepository<Bill>,        DataAccess.SQL.SQLRepository<Bill>>();
            container.RegisterType<IRepository<Category>,    DataAccess.SQL.SQLRepository<Category>>();
            container.RegisterType<IRepository<CompanyInfo>, DataAccess.SQL.SQLRepository<CompanyInfo>>();
            container.RegisterType<IRepository<Item>,        DataAccess.SQL.SQLRepository<Item>>();
            container.RegisterType<IRepository<Order>,       DataAccess.SQL.SQLRepository<Order>>();
            container.RegisterType<IRepository<Partner>,     DataAccess.SQL.SQLRepository<Partner>>();
            container.RegisterType<IRepository<BillItem>,    DataAccess.SQL.SQLRepository<BillItem>>();
            container.RegisterType<IRepository<CompanyItem>, DataAccess.SQL.SQLRepository<CompanyItem>>();
            container.RegisterType<IRepository<Orderitem>,   DataAccess.SQL.SQLRepository<Orderitem>>();
            container.RegisterType<IRepository<ItemInOrder>,   DataAccess.SQL.SQLRepository<ItemInOrder>>();
        }
    }
}