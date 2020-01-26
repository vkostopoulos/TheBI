using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using MongoDb.Repository;
using MongoDb.Services;
using MongoDb.Services.Impl;
using Newtonsoft.Json.Serialization;
using Owin;
using SimpleInjector;
using TheBigIdea;
using TheBigIdea.Helpers;
using TheBigIdea.Helpers.Impl;
using TheBigIdea.Ioc;

[assembly: OwinStartup(typeof(Startup))]

namespace TheBigIdea
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();

            app.UseWebApi(httpConfig);

            ConfigureAuth(app);

            Initialize();
        }

        public static void Initialize()
        {
            var container = BuildContainer();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorDependencyResolver(container);
        }

    

      

        public static Container BuildContainer()
        {
            var container = new Container();

            //repository
            container.Register(typeof(IRepository<>), typeof(MongoRepository<>), Lifestyle.Transient);
            container.Register<ICustomFieldsService, CustomFieldsService>(Lifestyle.Transient);
            container.Register<IContactsService, ContactsService>(Lifestyle.Transient);
            container.Register<IActionsService, ActionsService>(Lifestyle.Transient);
            container.Register<ICustomFieldValidator, CustomFieldValidator>(Lifestyle.Transient);
            container.Register<IActionValidator, ActionValidator>(Lifestyle.Transient);
            container.Register<IContactValidator, ContactValidator>(Lifestyle.Transient);
            container.Verify();
            return container;
        }

  
    }
}