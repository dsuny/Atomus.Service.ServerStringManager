using Atomus.Diagnostics;
using Atomus.Reflection;
using Atomus.Service.Controllers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Atomus.Service
{
    public class ServerStringManager : IServiceString
    {
        Assembly assembly;
        public ServerStringManager() { }
        
        string IServiceString.Request(string data)
        {
            //서비스 처리 dll 불러오기
            Atomus.MVVM.ViewModelExtensions viewModelExtensions;
            object parameter;
            object response;

            try
            {
                //Xamarin에서 만든 dll로 되는지 확인
                //if (assembly == null)
                //    assembly = LoadAssembly(32, 23, 1);
                //assembly = Assembly.Load(File.ReadAllBytes(@"E:\Work\Project\Atomus\개발\Management\Control\AccountTest\bin\Debug\Atomus.Management.Control.Account.dll"));

                //viewModelExtensions = (Atomus.MVVM.ViewModelExtensions)JsonConvert.DeserializeObject(data
                //    , typeof(Atomus.MVVM.ViewModelExtensions)
                //    , new JsonSerializerSettings
                //    {
                //        NullValueHandling = NullValueHandling.Include,
                //        MissingMemberHandling = MissingMemberHandling.Ignore,
                //        Formatting = Formatting.None,
                //        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                //        FloatParseHandling = FloatParseHandling.Decimal
                //    });

                //methodInfo = assembly.GetType(viewModelExtensions.MethodNamespace).GetMethod(viewModelExtensions.MethodName);

                //if (methodInfo != null && methodInfo.GetParameters()[0].ParameterType == typeof(ICore))
                //{

                //    parameter = JsonConvert.DeserializeObject(data
                //        , assembly.GetType(viewModelExtensions.ParameterType)
                //        , new JsonSerializerSettings
                //        {
                //            NullValueHandling = NullValueHandling.Include,
                //            MissingMemberHandling = MissingMemberHandling.Error,
                //            Formatting = Formatting.None,
                //            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                //            FloatParseHandling = FloatParseHandling.Decimal
                //        });
                //    ;

                //    response = methodInfo.Invoke(this, new object[] { null, parameter });
                //    return JsonConvert.SerializeObject(response);
                //}


                viewModelExtensions = Method.Execute(typeof(JsonConvert), "", "DeserializeObject"
                                                    ,  new object[] {
                                                        data
                                                        , typeof(Atomus.MVVM.ViewModelExtensions)
                                                        , new JsonSerializerSettings
                                                        {
                                                            NullValueHandling = NullValueHandling.Include,
                                                            MissingMemberHandling = MissingMemberHandling.Ignore,
                                                            Formatting = Formatting.None,
                                                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                                            FloatParseHandling = FloatParseHandling.Decimal
                                                        } }
                                                    , typeof(JsonSerializerSettings)
                                                    , new Atomus.MVVM.ViewModelExtensions()
                                                    , false);

                if (assembly == null)
                    assembly = LoadAssembly(viewModelExtensions.MenuID, viewModelExtensions.AssemblyID, viewModelExtensions.UserID);

                parameter = Method.Execute(typeof(JsonConvert), "", "DeserializeObject"
                                                    , new object[] {
                                                        data
                                                    , assembly.GetType(viewModelExtensions.ParameterType)
                                                    , new JsonSerializerSettings
                                                    {
                                                        NullValueHandling = NullValueHandling.Include,
                                                        MissingMemberHandling = MissingMemberHandling.Error,
                                                        Formatting = Formatting.None,
                                                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                                        FloatParseHandling = FloatParseHandling.Decimal
                                                    } }
                                                    , typeof(JsonSerializerSettings)
                                                    , assembly.CreateInstance(viewModelExtensions.ParameterType)
                                                    , false);


                response = Method.Execute(assembly.GetType(viewModelExtensions.MethodNamespace), "", viewModelExtensions.MethodName
                                                    , new object[] { null, parameter }
                                                    , (null as Type)
                                                    , assembly.CreateInstance(viewModelExtensions.ResultNamespace)
                                                    , false);

                return Method.Execute(typeof(JsonConvert), "", "SerializeObject"
                                                    , new object[] { response }
                                                    , (null as Type)
                                                    , ""
                                                    , false);

                //throw new AtomusException(string.Format("'{0}' Method not found", viewModelExtensions.MethodNamespace));
            }
            catch (Exception ex)
            {
                return string.Format("Atomus.Service.ServerStringManager : {0}", ex);
            }
        }

        Assembly LoadAssembly(decimal MENU_ID, decimal ASSEMBLY_ID, decimal USER_ID)
        {
            IResponse response;

            response = this.SearchUserControlAssembly(MENU_ID, ASSEMBLY_ID, USER_ID);

            return Assembly.Load(Convert.FromBase64String((string)response.DataSet.Tables[0].Rows[0]["FILE_TEXT"]));
        }
    }
}
