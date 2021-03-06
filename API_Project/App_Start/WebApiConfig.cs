﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_Project
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // - - - - - - - - - - - - - - - - - - - - -
            // - - - - - - - - - - - - - - - - - - - - -
            // Remove the JSON formatter
            // config.Formatters.Remove(config.Formatters.JsonFormatter);
            // or
            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            // - - - - - - - - - - - - - - - - - - - - -
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;

            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All; // Evitar referencia circular
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects; // Evitar referencia circular para c/objeto

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; // otra opcion de estructura de datos referenciados en json
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None; // otra opcion de estructura de datos referenciados en json

            // - - - - - - - - - - - - - - - - - - - - -
            // - - - - - - - - - - - - - - - - - - - - -

            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // http://api.eevapp.es/api/APPCTIVE/active/mgoncevatt.cep@gmail.com/123456789012345
            config.Routes.MapHttpRoute(
                name: "GetAPPCTIVATE",
                routeTemplate: "api/{controller}/active/{_email}/{_imei}",
                defaults: null
            );

            // http://api.eevapp.es/api/APPDATE/update/timestamp/123456789012345/1$2_1_1
            config.Routes.MapHttpRoute(
                name: "GetAPPDATE",
                routeTemplate: "api/{controller}/update/{_date}/{_imei}/{_search}",
                defaults: null
            );

            // http://api.eevapp.es/api/STATE/action/123456789012345
            config.Routes.MapHttpRoute(
                name: "GetSTATE",
                routeTemplate: "api/{controller}/action/{_imei}",
                defaults: null
            );

        }
    }
}