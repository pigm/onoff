using System;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BancoSecurityOnOff.Utilidades.Firmas;
using System.Collections.Generic;

/**
   @autor:          PaBlo Garrido
   @cargo:          Ingeniero de Software
   @institucion:    TInet soluciones informaticas
   @modificaciones: 
   @version:        1.0.0.0
   @proyecto:       OnOff
   
*/

namespace BancoSecurityOnOff.Droid.WebServiceSecurity
{
    public static class ServiciosSecurity
    {
        private const string Authorization = "Basic Sm8zTmNZR1QybXk0TU9DUXBhZzBua19LV2VnYTpKdDhSVTJTeHQ4cjFfSEdBeDdib3lTelVnbnNh";
        private const string POST = "POST";
        private const string Bearer = "Bearer ";
        private const string client_id = "c9daa363-735f-4b71-8afc-58163e457bef";
        private const string client_secret = "fX2sQ1uU4jD1aL4jS1mL0gG4bW2wG7lM4oD1nA2xY4lM5wV2bC";
        private const string contentTypeApplicationJson = "application/json";
        private const string contentTypeApplicationXwwwFormUrlencoded = "application/x-www-form-urlencoded";
        private static int TIMEOUT = 15000;

        public static async Task<JsonValue> CallRESTaccessToken()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SecurityEndpoints.URL_ACCESSTOKEN);
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationXwwwFormUrlencoded;
                request.Headers.Add("client_id", client_id);
                string grant_type = "client_credentials";
                 
                string postData = "grant_type=" + grant_type + "&client_id=" + client_id+ "&client_secret="+ client_secret +"&scope= BSecurity_OnOff";

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonValue.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service token. " + " An exception of type: " +ex);
                return string.Empty;
            }
        }

        public static JsonValue CallRESTToken()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SecurityEndpoints.URL_ACCESSTOKEN);
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationXwwwFormUrlencoded;
                request.Headers.Add("client_id", client_id);
            
                string grant_type = "client_credentials";
                string postData = "grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope= BSecurity_OnOff";

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                string content = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JsonValue releases = JsonObject.Parse(content);

                return releases;
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service token. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }
         
        public static async Task<JsonValue> CallRESTlogin(JsonValue token, string userRut, string userPassword, string typeUser, string nameChannel, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_LOGIN) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;
                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);
     
                Bean.LoginRequest loginRequest = new Bean.LoginRequest
                {
                        logHeader = new Bean.ParametriaLog{
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_LOGIN,
                        rut = userRut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    documentNumber = userRut.ToUpper(),
                    password = userPassword,
                    userType = typeUser,
                    channelName = nameChannel
                };


                string postData = JsonConvert.SerializeObject(loginRequest);
             

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service login. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTSMS(JsonValue token, string rut, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_SMS) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.SMSRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_SMS, // cambiar 
                        rut = rut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    rut = rut.ToUpper(),
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service llamar. " + " An exception of type: " + ex);
                return string.Empty;
            }

        }

        public static async Task<JsonValue> CallRESTAutenticarClaveVoz(JsonValue token, string aGrupoIDG, string aStrIdMecanismo, string aStrRutCliente, string aStrRutUsuario, string aStrPassword, string aCodigoTransaccion, string aStrSessionId1, string aStrSessionId2, string aIdLog, string aStrIP, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_AUTENTICAR_CLAVEVOZ) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.AutenticarClaveVozRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_AUTENTICAR,
                        rut = aStrRutCliente.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    strGrupoIDG = aGrupoIDG,
                    strIdMecanismo = aStrIdMecanismo,
                    strRutCliente = aStrRutCliente.ToUpper(),
                    strRutUsuario = aStrRutUsuario.ToUpper(),
                    strPassword = aStrPassword,
                    codigoTransaccion = aCodigoTransaccion,
                    strSessionId1 = aStrSessionId1,
                    strSessionId2 = aStrSessionId2,
                    idLog = aIdLog,
                    strIP = aStrIP
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service autenticación por voz. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }


        public static async Task<JsonValue> CallRESTConsultaEnrrolado(JsonValue token, string imeiDispocitivo, string sistemaOperativo, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CONSULTAENROLADO) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.ConsultaEnrolado
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CONSULTAENROLADO,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    idDispositivo = imeiDispocitivo,
                    sistema = sistemaOperativo

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service consulta enrolado. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTEnrolarDispositivo(JsonValue token, string en_id, string en_rut, string en_sistema, string en_canal, string en_estado, string en_sistemaOperativo, string en_alias, string en_nombreDispositivo, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_ENROLARDISPOSITIVO) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.EnrolarDispositivoRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_ENROLARDISPOSITIVO,
                        rut = en_rut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    idDispositivo = en_id,
                    rut = en_rut.ToUpper(),
                    sistema = en_sistema,
                    canal = en_canal,
                    estado = en_estado,
                    sistemaOperativo = en_sistemaOperativo,
                    alias = en_alias,
                    nombreDispositivo = en_nombreDispositivo

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service enrolar dispositivo. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTConsultaProductoNotificaciones(JsonValue token, string cpnFlagInicio, string cpnTamanoBloque, string cpnPaginaConsultar, string cpnNumeroRegistros, string cpnTotalPaginas, string cpnUltimoRegistro, string cpnRut, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
				HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CONSULTAPRODUCTONOTIFICACIONES) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

				string postData = JsonConvert.SerializeObject(new Bean.ConsultaProductosNotificacionesRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CONSULTAPRODUCTONOTIFICACIONES,
                        rut = cpnRut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
					flagInicio = cpnFlagInicio,
                    tamanoBloque = cpnTamanoBloque,
                    paginaConsultar = cpnPaginaConsultar,
                    numeroRegistros = cpnNumeroRegistros,
                    totalPaginas = cpnTotalPaginas,
                    ultimoRegistro = cpnUltimoRegistro,
                    rut = cpnRut.ToUpper()
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service enrolar dispositivo. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static JsonValue CallRESTGetCardData(JsonValue token, string gcdRut, string gcdPan, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_GETCARDDATA) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.GetCardDataRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_GETCARDDATA,
                        rut = gcdRut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    rut = gcdRut.ToUpper(),
                    pan = gcdPan,

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                string content = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JsonValue releases = JsonObject.Parse(content);

                return releases;
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service Get Card Data. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTSetEnable(JsonValue token, string sePan, string seEstado, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_SETENABLE) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.SetEnabledRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_SETENABLE,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    pan = sePan,
                    estado = seEstado

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service SetEnable. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTChangePaymentChannels(JsonValue token, string cpcPan, string cpcEstadoPos, string cpcEstadoAtm, string cpcEstadoEcom, string cpcEstadoMoto, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CHANGEPAYMENTCHANNELS) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.ChangePaymentChannelsRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CHANGEPAYMENTCHANNELS,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    pan = cpcPan,
                    estadoPos = cpcEstadoPos,
                    estadoAtm = cpcEstadoAtm,
                    estadoEcom = cpcEstadoEcom,
                    estadoMoto = cpcEstadoMoto
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service Change Payment Channels. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static JsonValue CallRESTCheckCardStatus(JsonValue token, string cceFi, string cceAbaBranch, string ccePan, string cceAccountType, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CHECKCARDSTATUS) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.CheckCardStatusRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CHECKCARDSTATUS,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    fi = cceFi,
                    abaBranch = cceAbaBranch,
                    pan = ccePan,
                    accountType = cceAccountType
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                string content = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JsonValue releases = JsonObject.Parse(content);

                return releases;
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service Check Card Status. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static async Task<JsonValue> CallRESTOnOffCardStatus(JsonValue token, string oceFi, string oceAbaBranch, string ocePan, string oceAccountType, string oceStatus, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_ONOFFCARDSTATUS) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.OnOffCardStatusRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_ONOFFCARDSTATUS,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    fi = oceFi,
                    abaBranch = oceAbaBranch,
                    pan = ocePan,
                    accountType = oceAccountType,
                    status = oceStatus

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonObject.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service on off Card Status. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public static JsonValue CallRESTConsultaNotificaciones(JsonValue token, string cnRut, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CONSULTANOTIFICACIONES) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.ConsultaNotificacionesRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CONSULTANOTIFICACIONES,
                        rut = cnRut.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    rut = cnRut.ToUpper()

                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                string content = string.Empty;
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }

                JsonValue releases = JsonObject.Parse(content);

                return releases;
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service Consulta notificaciones. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public async static Task<JsonValue> CallRESTInsertaProductoNotificaciones(JsonValue token, List<Bean.IngresarProductosNotificacionesRequest> listaProductoNotificaciones, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_INGRESAPRODUCTONOTIFICACIONES) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postDataRoot = JsonConvert.SerializeObject(new Bean.IngresarProductosNotificacionesRoot
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_INGRESARPRODUCTONOTIFICACIONES,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    notificaciones = new Bean.ListIngresaProductoNotificacion(listaProductoNotificaciones)
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postDataRoot);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonValue.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service ingresa producto notificaciones. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public async static Task<JsonValue> CallRESTInsertaNotificaciones(JsonValue token, List<Bean.IngresarNotificacionesRequest> listaNotificaciones, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_INGRESANOTIFICACIONES) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = contentTypeApplicationJson;
                string access_token = Bearer + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postDataRoot = JsonConvert.SerializeObject(new Bean.IngresarNotificacionesRoot
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_INGRESARNOTIFICACIONES,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    Notificaciones = new Bean.ListIngresaNotificacion(listaNotificaciones)
                });

                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postDataRoot);
                    }
                }

                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonValue.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service ingresa notificaciones. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }

        public async static Task<JsonValue> CallRESTConsultaHistorialNotificaciones(JsonValue token, string chnRut, string run, string idDispositivoApp, string ipOrigenApp)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(SecurityEndpoints.URL_CONSULTAHISTORIALNOTIFICACIONES) as HttpWebRequest;
                request.Method = POST;
                request.Timeout = TIMEOUT;
                request.ContentType = "application/json";

                string access_token = "Bearer " + token;

                request.Headers.Add("Authorization", access_token);
                request.Headers.Add("X-IBM-Client-Id", client_id);
                request.Headers.Add("client_secret", client_secret);

                string postData = JsonConvert.SerializeObject(new Bean.NotificacionesRequest
                {
                    logHeader = new Bean.ParametriaLog
                    {
                        service = ConstantesSecurity.CODIGO_LOG_SERVICE_CONSULTAHISTORIALNOTIFICACIONES,
                        rut = run.ToUpper(),
                        idDispositivo = idDispositivoApp,
                        ipOrigen = ipOrigenApp
                    },
                    rut = chnRut.ToUpper()

                });
                using (Stream dataStream = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(dataStream))
                    {
                        stmw.Write(postData);
                    }
                }
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        JsonValue json = await Task.Run(() => JsonValue.Load(reader));
                        return json;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("App on - off has not been able to establish communication with the web service Consulta historial notificaciones. " + " An exception of type: " + ex);
                return string.Empty;
            }
        }
    }
}
