namespace JelloScrum.Web
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using System.Web;
    using Castle.ActiveRecord;
    using Castle.ActiveRecord.Framework;
    using Castle.ActiveRecord.Framework.Config;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.Framework.Configuration;
    using Castle.MonoRail.Framework.Container;
    using Castle.MonoRail.Framework.Helpers.ValidationStrategy;
    using Castle.MonoRail.Framework.JSGeneration;
    using Castle.MonoRail.Framework.JSGeneration.jQuery;
    using Castle.MonoRail.Framework.Services.AjaxProxyGenerator;
    using Castle.Windsor;
    using Container;


    /// <summary>
    /// 
    /// </summary>
    public class Global : HttpApplication, IMonoRailContainerEvents, IMonoRailConfigurationEvents, IContainerAccessor
    {
        private static IWindsorContainer container;

        /// <summary>
        /// Sets the culture to dutch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //public override void UnitOfWorkApplication_BeginRequest(object sender, EventArgs e)
        //{
        //    base.UnitOfWorkApplication_BeginRequest(sender, e);
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo("nl-NL");
        //}

        protected void Application_Start(object sender, EventArgs e)
        {
            #region Container

            container = IoC.Initialize();

          //  IEmailTemplateService toet = IoC.Resolve<IEmailTemplateService>();

            #endregion

            #region ActiveRecord

            IConfigurationSource config = ActiveRecordSectionHandler.Instance;
            Assembly asm1 = Assembly.Load("JelloScrum.Model");

            ActiveRecordStarter.Initialize(new Assembly[] { asm1 }, config);

            #endregion
        }

        protected void Application_End(object sender, EventArgs e)
        {
            container.Dispose();
        }

        ///<summary>
        /// Gives implementors a chance to register services into MonoRail's container.
        ///</summary>
        ///<param name="container">The container.</param>
        public void Created(IMonoRailContainer container)
        {
        }

        ///<summary>
        /// Gives implementors a chance to get MonoRail's services and uses it somewhere else - 
        /// for instance, registering them on an IoC container.
        ///</summary>
        ///<param name="container"></param>
        public void Initialized(IMonoRailContainer container)
        {
            IAjaxProxyGenerator ajaxProxyGenerator = new JQueryAjaxProxyGenerator();
            container.ServiceInitializer.Initialize(ajaxProxyGenerator, container);
            container.AjaxProxyGenerator = ajaxProxyGenerator;
        }

        ///<summary>
        /// Implementors can take a chance to change MonoRail's configuration.
        ///</summary>
        ///<param name="configuration">The configuration.</param>
        public void Configure(IMonoRailConfiguration configuration)
        {
            configuration.JSGeneratorConfiguration.AddLibrary("jquery-1.3.2", typeof (JQueryGenerator))
                .AddExtension(typeof (CommonJSExtension))
                .ElementGenerator
                .AddExtension(typeof (JQueryElementGenerator))
                .Done
                .BrowserValidatorIs(typeof (JQueryValidator))
                .SetAsDefault();
        }

        public IWindsorContainer Container
        {
            get { return container; }
        }
    }
}