using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Duoju.DAO.Utilities
{
    public class HttpWebResponseUtility
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = DefaultUserAgent;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        public static string GetResponseContent(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            var streamReader = new StreamReader(stream);
            var responseContent = streamReader.ReadToEnd();
            streamReader.Close();
            stream.Close();
            return responseContent;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }


        public static CookieCollection CookieStrToCookieCollection(string s, string defaultDomain)
        {
            CookieCollection cc = new CookieCollection();
            if (string.IsNullOrEmpty(s) || s.Length < 5 || s.IndexOf("=") < 0) return cc;
            if (string.IsNullOrEmpty(defaultDomain) || defaultDomain.Length < 5) return cc;
            s.TrimEnd(new char[] { ';' }).Trim();
            Uri urI = new Uri(defaultDomain);
            defaultDomain = urI.Host.ToString();
            if (s.IndexOf("expires=") >= 0)
            {
                s = replace(s, @"expires=[\w\s,-:]*GMT[;]?", "");
            }
            if (s.IndexOf(";") < 0)
            {
                System.Net.Cookie c = new System.Net.Cookie(s.Substring(0, s.IndexOf("=")), s.Substring(s.IndexOf("=") + 1));
                c.Domain = defaultDomain;
                cc.Add(c);
                return cc;
            }
            if (s.IndexOf(",") > 0)
            {
                s.TrimEnd(new char[] { ',' }).Trim();
                foreach (string s2 in s.Split(','))
                {
                    cc = strCokAddCol(s2, defaultDomain, cc);
                }
                return cc;
            }
            else
            {
                return strCokAddCol(s, defaultDomain, cc);
            }
        }

        private static CookieCollection strCokAddCol(string s, string defaultDomain, CookieCollection cc)
        {
            try
            {
                s.TrimEnd(new char[] { ';' }).Trim();
                System.Collections.Hashtable hs = new System.Collections.Hashtable();
                foreach (string s2 in s.Split(';'))
                {
                    string s3 = s2.Trim();
                    if (s3.IndexOf("=") > 0)
                    {
                        string[] s4 = s3.Split('=');
                        hs.Add(s4[0].Trim(), s4[1].Trim());
                    }
                }
                string defaultPath = "/";
                foreach (object Key in hs.Keys)
                {
                    if (Key.ToString().ToLower() == "path")
                    {
                        defaultPath = hs[Key].ToString();
                    }
                    else if (Key.ToString().ToLower() == "domain")
                    {
                        defaultDomain = hs[Key].ToString();
                    }
                }
                foreach (object Key in hs.Keys)
                {
                    if (!string.IsNullOrEmpty(Key.ToString()) && !string.IsNullOrEmpty(hs[Key].ToString()))
                    {
                        if (Key.ToString().ToLower() != "path" && Key.ToString().ToLower() != "domain")
                        {
                            Cookie c = new Cookie();
                            c.Name = Key.ToString();
                            c.Value = hs[Key].ToString();
                            c.Path = defaultPath;
                            c.Domain = defaultDomain;
                            cc.Add(c);
                        }
                    }
                }
            }
            catch { }
            return cc;
        }

        public static string replace(string strSource, string strRegex, string strReplace)
        {
            try
            {
                Regex r;
                r = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string s = r.Replace(strSource, strReplace);
                return s;
            }
            catch
            {
                return strSource;
            }
        }
    }
}
